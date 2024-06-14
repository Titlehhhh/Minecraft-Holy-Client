namespace McProtoNet;

public class DisconnectException : Exception
{
    public DisconnectException(string message) : base(message)
    {
    }
}