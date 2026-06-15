using Game.Network.IO;
using Game.Packets.Request;
using Game.Utils;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Network {
    public class NettyManager : MonoBehaviour {

        public static NettyManager Instance {
            get; private set;
        }

        public PacketDispatcher Dispatcher { get; } = new();

        [Header("Network Settings")]
        public string serverIP = "218.54.49.3";
        public int serverPort = 8484;

        private bool isInitialized = false;

        private TcpClient client;
        private NetworkStream stream;
        private CancellationTokenSource ctx;

        private readonly ConcurrentQueue<byte[]> sendQueue = new();
        private readonly ConcurrentQueue<byte[]> receiveQueue = new();
        private readonly SemaphoreSlim sendLock = new(1, 1);

        private void Awake() {
            if (Instance == null) {
                //foreach (LogType type in Enum.GetValues(typeof(LogType))) {
                //    PlayerSettings.SetStackTraceLogType(type, StackTraceLogType.None);
                //}
                Instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }

        private void OnEnable() {
            if (!isInitialized) {
                InitializeNetwork();
            }
        }

        private void InitializeNetwork() {
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes()) {
                if (type.GetCustomAttribute<PacketHandlerAttribute>() != null && typeof(IPacketHandler).IsAssignableFrom(type)) {
                    var handler = (IPacketHandler)Activator.CreateInstance(type);
                    handler.Register(this);
                }
            }
            ConnectToServer();
            isInitialized = true;
        }

        private void OnDisable() {
            isInitialized = false;
            Close();
        }

        private void OnApplicationQuit() {
            isInitialized = false;
            Close();
        }

        public void ConnectToServer() {
            ctx?.Cancel();
            ctx = new CancellationTokenSource();
            _ = ConnectAsync(ctx.Token);
        }

        private async Task ConnectAsync(CancellationToken token) {
            while (!token.IsCancellationRequested) {
                try {
                    client = new TcpClient();
                    await client.ConnectAsync(serverIP, serverPort);
                    stream = client.GetStream();
                    OnConnected();
                    await ReceiveLoop(token);
                } catch (Exception e) {
                    Debug.LogWarning($"[NET] 연d결 실패: {e.Message}");
                }
                await Task.Delay(3000, token);
            }
        }

        private async Task ReceiveLoop(CancellationToken token) {
            byte[] lengthBuffer = new byte[4];
            while (!token.IsCancellationRequested && client?.Connected == true) {
                if (!await ReadExact(lengthBuffer, 4, token)) {
                    break;
                }
                int len = BitConverter.ToInt32(lengthBuffer, 0);
                if (len <= 0 || len > 1024 * 1024) {
                    continue;
                }
                byte[] rentBuffer = ArrayPool<byte>.Shared.Rent(len);
                try {
                    if (!await ReadExact(rentBuffer, len, token)) {
                        break;
                    }
                    byte[] packet = new byte[len];
                    Buffer.BlockCopy(rentBuffer, 0, packet, 0, len);
                    receiveQueue.Enqueue(packet);
                } finally {
                    ArrayPool<byte>.Shared.Return(rentBuffer);
                }
            }
        }

        private async Task<bool> ReadExact(byte[] buffer, int size, CancellationToken token) {
            int totalRead = 0;
            while (totalRead < size) {
                int read = await stream.ReadAsync(buffer.AsMemory(totalRead, size - totalRead), token);
                if (read == 0) {
                    return false;
                }
                totalRead += read;
            }
            return true;
        }

        public void Send(byte[] data) {
            sendQueue.Enqueue(data);
            if (stream != null && client != null && client.Connected) {
                _ = ProcessSendQueue();
            } else {
                Debug.Log("[NET] 연결 준비 중... 패킷을 큐에 저장했습니다.");
            }
        }

        private async Task ProcessSendQueue() {
            if (!await sendLock.WaitAsync(0))
                return;
            try {
                while (sendQueue.TryDequeue(out var data)) {
                    if (stream != null && client.Connected) {
                        await stream.WriteAsync(data, 0, data.Length);
                    }
                }
                await stream.FlushAsync();
            } finally {
                sendLock.Release();
            }
        }

        private void Update() {
            while (receiveQueue.TryDequeue(out var packet)) {
                Dispatcher.Dispatch(packet);
            }
        }

        public void RegisterHandler(short opcode, Action<LittleEndianReader> handler) {
            Dispatcher.RegisterHandler(opcode, handler);
        }

        private void Close() {
            ctx?.Cancel();
            stream = stream.DisposeAndNull();
            client = client.DisposeAndNull();
        }

        private void OnConnected() {
            Debug.Log("서버 연결 성공! 패킷 전송을 준비합니다.");
            Send(LoginPacket.getCharacterInformation(1));
        }
    }
}