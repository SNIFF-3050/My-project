public enum ClientSendOpcode : short {
    PONG = 1,
    LOAD_FROM_CHARACTER = 2,
    GOLD_UPDATE = 3,
}

public static class ClientSendOpcodeExtensions {
    public static short GetValue(this ClientSendOpcode opcode) {
        return (short)opcode;
    }
}