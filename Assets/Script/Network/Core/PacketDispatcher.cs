using Game.Network.IO;
using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace Game.Network {
    public class PacketDispatcher {
        private readonly ConcurrentDictionary<short, Action<LittleEndianReader>> _handlers = new();
        public void RegisterHandler(short opcode, Action<LittleEndianReader> handler) => _handlers.TryAdd(opcode, handler);
        public void Dispatch(byte[] packet) {
            try {
                var reader = new LittleEndianReader(packet);
                short opcode = reader.ReadShort();
                if (_handlers.TryGetValue(opcode, out var handler)) {
                    handler(reader);
                } else {
                    Debug.LogError($"[Dispatcher] 처리되지 않은 Opcode: {opcode}");
                }
            } catch (Exception e) {
                Debug.LogError($"[Dispatcher] 오류: {e.Message}");
            }
        }
    }
}