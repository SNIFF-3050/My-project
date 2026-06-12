using System;

namespace Game.Network {
    [AttributeUsage(AttributeTargets.Class)]
    public class PacketHandlerAttribute : Attribute {
    }

    public interface IPacketHandler {
        void Register(NettyManager manager);
    }
}