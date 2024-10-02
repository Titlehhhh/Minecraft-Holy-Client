namespace McProtoNet.Abstractions;

public sealed class StateEventArgs : EventArgs
{
    public StateEventArgs(MinecraftClientState oldState, MinecraftClientState newState)
    {
        State = newState;
        OldState = oldState;
    }

    public MinecraftClientState State { get; }
    public MinecraftClientState OldState { get; }
}

public sealed class DisconnectedEventArgs : EventArgs
{
    public DisconnectedEventArgs(Exception? exception = null)
    {
        Exception = exception;
    }

    public Exception? Exception { get; }
}