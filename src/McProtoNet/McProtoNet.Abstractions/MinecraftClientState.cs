namespace McProtoNet.Abstractions;

public enum MinecraftClientState
{
    Stopped,
    Connect,
    Errored,
    Handshaking,
    Login,
    Play
}