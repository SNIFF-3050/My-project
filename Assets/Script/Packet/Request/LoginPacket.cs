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

        public static byte[] getCharacterInformation(int accountid) {
            var writer = new LittleEndianWriter();
            writer.WriteShort(ClientSendOpcode.LOAD_FROM_CHARACTER.GetValue());
            writer.WriteInt(accountid);
            return writer.GetPacketBytes();
        }

        public static byte[] getGoldInformation(long gold) {
            var writer = new LittleEndianWriter();
            writer.WriteShort(ClientSendOpcode.GOLD_UPDATE.GetValue());
            writer.WriteLong(gold);
            return writer.GetPacketBytes();
        }
    }
}