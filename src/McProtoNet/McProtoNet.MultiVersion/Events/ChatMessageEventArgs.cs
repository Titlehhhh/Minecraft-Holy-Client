namespace McProtoNet.MultiVersion.Events
{

    public class SpawnEntityEventArgs : EventArgs
    {
        public int Id { get; private set; }
        public Guid UUID { get; private set; }

        public int MyProperty { get; private set; }

    }
    public class ChatMessageEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public ChatMessageEventArgs(string message)
        {
            Message = message;
        }
    }
}
