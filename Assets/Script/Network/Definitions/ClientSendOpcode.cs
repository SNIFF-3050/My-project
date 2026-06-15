public enum ClientSendOpcode : short {
    PONG = 1,
    TEST = 2,
}

public static class ClientSendOpcodeExtensions {
    public static short GetValue(this ClientSendOpcode opcode) {
        return (short)opcode;
    }
}