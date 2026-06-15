using Game.Network;
using Game.Network.IO;
using System.IO;
using UnityEngine;

[PacketHandler]
public class LoginHandler : IPacketHandler {
    public void Register(NettyManager manager) {
        manager.RegisterHandler((short)ClientRecvOpcode.PING, getServerPong);
        manager.RegisterHandler((short)ClientRecvOpcode.UI_GOLD, HandlerUpdateGoldStatus);
        manager.RegisterHandler((short)ClientRecvOpcode.LOAD_FROM_CHARACTER, getCharactrerInformation);
        Debug.Log("LoginHandler: 자동 자ㅓ동 완료등록 완료");
    }

    private void getServerPong(LittleEndianReader reader) {
        try {
        } catch (EndOfStreamException e) {
            Debug.Log(e.Message);
        }
    }

    private void getCharactrerInformation(LittleEndianReader reader) {
        try {
            Debug.Log("캐릭터 정보 불러오는중");
            GameClient.Instance.LoadFromCharacter(reader);
        } catch (EndOfStreamException e) {
            Debug.Log("GD" + e.Message);
        }
    }

    private void HandlerUpdateGoldStatus(LittleEndianReader reader) {
        try {
            long gold = reader.ReadLong();
            if (GameClient.Instance != null) {
                GameClient.Instance.getGoldInformation(gold, false);
            }
        } catch (EndOfStreamException e) {
            Debug.Log(e.Message);
        }
    }

}