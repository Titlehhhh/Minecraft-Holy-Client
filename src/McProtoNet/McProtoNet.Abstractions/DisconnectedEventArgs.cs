namespace McProtoNet.Abstractions;

public sealed class DisconnectedEventArgs : EventArgs
{
    public DisconnectedEventArgs(Exception? exception, MinecraftClientState previousState)
    {
        PreviousState = previousState;
        Exception = exception;
    }

    public MinecraftClientState PreviousState { get; }
    public Exception? Exception { get; }
}