namespace McProtoNet.MultiVersion.Client
{
    public sealed class StateChangedEventArgs
    {
        public StateChangedEventArgs(ClientState oldState, ClientState newState)
        {
            OldState = oldState;
            NewState = newState;
        }

        public ClientState OldState { get; }
        public ClientState NewState { get; }
    }
}