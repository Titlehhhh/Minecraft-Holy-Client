namespace McProtoNet.Abstractions;

public sealed class StateEventArgs : EventArgs
{
    public StateEventArgs(MinecraftClientState oldState, MinecraftClientState newState)
    {
        State = newState;
        OldState = oldState;
    }

    public StateEventArgs(Exception ex, MinecraftClientState oldState, MinecraftClientState newState)
    {
        Error = ex;
        State = newState;
        OldState = oldState;
    }

    public StateEventArgs(Exception ex, MinecraftClientState oldState)
    {
        Error = ex;
        State = MinecraftClientState.Errored;
        OldState = oldState;
    }

    public MinecraftClientState State { get; }
    public MinecraftClientState OldState { get; }

    public Exception? Error { get; }
}