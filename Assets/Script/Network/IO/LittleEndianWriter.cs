using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Game.Network.IO {
    public class LittleEndianWriter {
        private MemoryStream stream;
        private BinaryWriter writer;

        public LittleEndianWriter() {
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
        }

        public void WriteByte(byte value) => writer.Write(value);
        public void WriteShort(short value) => writer.Write(value);
        public void WriteInt(int value) => writer.Write(value);
        public void WriteLong(long value) => writer.Write(value);

        public void WritePos(UnityEngine.Vector2Int pos) {
            writer.Write((short)pos.x);
            writer.Write((short)pos.y);
        }

        public void WriteAsciiString(string value) {
            if (string.IsNullOrEmpty(value)) {
                writer.Write((short)0);
                return;
            }
            byte[] stringBytes = Encoding.GetEncoding("UTF-8").GetBytes(value);
            writer.Write((short)stringBytes.Length);
            writer.Write(stringBytes);
        }

        public byte[] GetPacketBytes() {
            writer.Flush();
            byte[] bodyBytes = stream.ToArray();

            int packetLength = bodyBytes.Length;
            byte[] headerBytes = BitConverter.GetBytes(packetLength);

            if (!BitConverter.IsLittleEndian)
                Array.Reverse(headerBytes);

            byte[] fullPacket = new byte[headerBytes.Length + bodyBytes.Length];
            Buffer.BlockCopy(headerBytes, 0, fullPacket, 0, headerBytes.Length);
            Buffer.BlockCopy(bodyBytes, 0, fullPacket, headerBytes.Length, bodyBytes.Length);

            return fullPacket;
        }

        public void Close() {
            writer.Close();
            stream.Close();
        }

        private static string ToString(byte[] bytes) {
            if (bytes == null || bytes.Length == 0) {
                return string.Empty;
            }
            return string.Join(" ", bytes.Select(b => b.ToString("X2")));
        }

        public string getPacketString() {
            writer.Flush();
            byte[] bytes = stream.ToArray();
            return ToString(bytes);
        }
    }
}