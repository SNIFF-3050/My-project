public enum ClientRecvOpcode : short
{
    PING = 1,
    UI_GOLD = 2,
    LOAD_FROM_CHARACTER = 3,
}

public static class ClientRecvOpcodeExtensions {
    public static short GetValue(this ClientRecvOpcode opcode) {
        return (short)opcode;
    }
}