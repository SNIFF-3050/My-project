using Game.Network;
using Game.Network.IO;
using System.IO;
using UnityEngine;

[PacketHandler]
public class LoginHandler : IPacketHandler {
    public void Register(NettyManager manager) {
        manager.RegisterHandler((short)ClientRecvOpcode.PING, Pong);
        manager.RegisterHandler((short)ClientRecvOpcode.LOGIN_STATUS, HandlerLoginStatus);
        Debug.Log("LoginHandler: 자동 자ㅓ동 완료등록 완료");
    }
    
    private void Pong(LittleEndianReader reader) {
        try {
            Debug.Log("리시브ggaaaaaaaaa : " + reader.ReadLong());
            Debug.Log("리시브ggSaaaa : " + reader.ReadLong());
            Debug.Log("리시브 받음2111111111222222222222222" + reader.getPacketString());
            Debug.Log("리시브 받음2" + reader.getPacketString());
            Debug.Log("리시브 : " + reader.ReadAsciiString());
            Debug.Log("리시브 : " + reader.ReadAsciiString());
            Debug.Log("리시브 : " + reader.ReadAsciiString());
            Debug.Log("리시브 : " + reader.ReadAsciiString());
            Debug.Log("리시브 받음22222A22222222222" + reader.getPacketString());
            Debug.Log("리시브 받음22222211111111111111111111111111111111" + reader.getPacketString());
            Debug.Log("리시브 : " + reader.ReadLong());
            Debug.Log("리시브 : " + reader.ReadInt());
            Debug.Log("리시브 : " + reader.ReadLong());
            Debug.Log("리시aa브 : " + reader.ReadAsciiString());
            Debug.Log("리시브 : " + reader.ReadLong());
            Debug.Log("리시브 : " + reader.ReadByte());
            Debug.Log("리시브 받음2222222222222222" + reader.getPacketString());
        } catch (EndOfStreamException e) {
            Debug.Log("GD" + e.Message);
        }
    }

    private void HandlerLoginStatus(LittleEndianReader reader) => Debug.Log("로그인 상태 리시브 : " + reader.getPacketString());
}