public enum ClientRecvOpcode : short
{
    PING = 1,
    LOGIN_STATUS = 2,
    TEST = 3,
}

public static class ClientRecvOpcodeExtensions {
    public static short GetValue(this ClientRecvOpcode opcode) {
        return (short)opcode;
    }
}