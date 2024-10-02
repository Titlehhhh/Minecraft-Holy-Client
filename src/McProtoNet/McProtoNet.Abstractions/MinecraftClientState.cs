namespace McProtoNet.Abstractions;

public enum MinecraftClientState
{
    Disconnected = 0,
    Disconnecting,
    Connect,
    Handshaking,
    Login,
    Play
}