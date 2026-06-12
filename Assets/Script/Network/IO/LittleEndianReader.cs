using System;
using System.IO;
using System.Text;

namespace Game.Network.IO {
    public class LittleEndianReader {
        private readonly byte[] buffer;
        private int position;

        public LittleEndianReader(byte[] packetBytes) {
            this.buffer = packetBytes;
            this.position = 0;
        }

        public int ReadByte() {
            if (position >= buffer.Length)
                throw new EndOfStreamException();
            return buffer[position++];
        }

        public bool ReadBool() {
            return ReadByte() == 1;
        }

        public short ReadShort() {
            int byte1 = ReadByte();
            int byte2 = ReadByte();
            return (short)((byte2 << 8) + byte1);
        }

        public int ReadInt() {
            int byte1 = ReadByte();
            int byte2 = ReadByte();
            int byte3 = ReadByte();
            int byte4 = ReadByte();
            return (byte4 << 24) + (byte3 << 16) + (byte2 << 8) + byte1;
        }

        public long ReadLong() {
            long byte1 = ReadByte();
            long byte2 = ReadByte();
            long byte3 = ReadByte();
            long byte4 = ReadByte();
            long byte5 = ReadByte();
            long byte6 = ReadByte();
            long byte7 = ReadByte();
            long byte8 = ReadByte();
            return (long)((byte8 << 56) + (byte7 << 48) + (byte6 << 40) + (byte5 << 32) + (byte4 << 24) + (byte3 << 16) + (byte2 << 8) + byte1);
        }

        public UnityEngine.Vector2Int ReadPos() {
            short x = ReadShort();
            short y = ReadShort();
            return new UnityEngine.Vector2Int(x, y);
        }

        public UnityEngine.Vector2Int ReadPosInt() {
            int x = ReadInt();
            int y = ReadInt();
            return new UnityEngine.Vector2Int(x, y);
        }

        public string ReadAsciiString() {
            short stringLength = ReadShort();
            if (stringLength <= 0 || position + stringLength > buffer.Length) {
                return "";
            }
            byte[] strBytes = ReadBytes(stringLength);
            return Encoding.GetEncoding("UTF-8").GetString(strBytes);
        }

        public byte[] ReadBytes(int count) {
            if (position + count > buffer.Length) {
                count = buffer.Length - position;
            }
            if (count <= 0) {
                return new byte[0];
            }
            byte[] result = new byte[count];
            Buffer.BlockCopy(buffer, position, result, 0, count);
            position += count;
            return result;
        }

        public int Available() => buffer.Length - position;

        private static readonly char[] HEX = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        public static string ToHex(byte byteValue) {
            int tmp = byteValue << 8;
            char[] retstr = new char[] { HEX[(tmp >> 12) & 0x0F], HEX[(tmp >> 8) & 0x0F] };
            return new string(retstr);
        }

        public static string ToHex(byte[] bytes) {
            if (bytes == null || bytes.Length == 0) {
                return string.Empty;
            }
            StringBuilder hexed = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++) {
                hexed.Append(ToHex(bytes[i]));
                hexed.Append(' ');
            }
            return hexed.ToString(0, hexed.Length - 1);
        }

        public string getPacketString() {
            string nows = "";
            int remaining = buffer.Length - position; 
            if (remaining > 0) {
                byte[] now = new byte[remaining];
                Buffer.BlockCopy(buffer, position, now, 0, remaining);
                nows = ToHex(now);
            }
            return "Data: " + nows;
        }
    }
}