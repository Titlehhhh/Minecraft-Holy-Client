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