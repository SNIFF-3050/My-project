using Game.Network;
using Game.Network.IO;
using Game.Packets.Request;
using System.IO;
using UnityEngine;

[PacketHandler]
public class LoginHandler : IPacketHandler {
    public void Register(NettyManager manager) {
        manager.RegisterHandler((short)ClientRecvOpcode.PING, Pong);
        manager.RegisterHandler((short)ClientRecvOpcode.LOGIN_STATUS, HandlerLoginStatus);
        manager.RegisterHandler((short)ClientRecvOpcode.UI_GOLD, HandlerUpdateGoldStatus);
        Debug.Log("LoginHandler: 자동 자ㅓ동 완료등록 완료");
    }
    
    private void Pong(LittleEndianReader reader) {
        try {
            Debug.Log("리시브ggaaaaaaaaa : " + reader.ReadLong());
        } catch (EndOfStreamException e) {
            Debug.Log("GD" + e.Message);
        }
    }

    private void HandlerLoginStatus(LittleEndianReader reader) => Debug.Log("로그인 상태 리시브 : " + reader.getPacketString());

    private void HandlerUpdateGoldStatus(LittleEndianReader reader) {
        try {
            long gold = reader.ReadLong();
            MonsterController.Instance.AddGold(gold);
            NettyManager.Instance.Send(LoginPacket.getTest());
        } catch (EndOfStreamException e) {
            Debug.Log("GD" + e.Message);
        }
    }

}