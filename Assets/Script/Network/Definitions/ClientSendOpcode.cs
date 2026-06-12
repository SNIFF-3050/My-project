public enum ClientSendOpcode : short
{
    PONG = 1,
}

public static class ClientSendOpcodeExtensions {
    public static short GetValue(this ClientSendOpcode opcode) {
        return (short)opcode;
    }
}