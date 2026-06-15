using Game.Network.IO;

namespace Game.Packets.Request {
    public class LoginPacket {

        public static byte[] getPong() {
            var writer = new LittleEndianWriter();
            writer.WriteShort(ClientSendOpcode.PONG.GetValue());
            writer.WriteInt(1);
            writer.WriteInt(1);
            return writer.GetPacketBytes();
        }

        public static byte[] getTest() {
            var writer = new LittleEndianWriter();
            writer.WriteShort(ClientSendOpcode.TEST.GetValue());
            writer.WriteLong(1323);
            return writer.GetPacketBytes();
        }
    }
}