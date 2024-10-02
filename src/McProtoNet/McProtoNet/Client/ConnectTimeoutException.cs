namespace McProtoNet.Client;

public class ConnectTimeoutException : Exception
{
    public ConnectTimeoutException()
    {
        
    }

    public ConnectTimeoutException(string? message):base(message)
    {
        
    }
    public ConnectTimeoutException(string? message, Exception? innerException) : base(message, innerException)
    {
        
    }
    
}

public class ConnectException : Exception
{
    public ConnectException()
    {
    }

    public ConnectException(string message) : base(message)
    {
    }

    public ConnectException(string message, Exception inner) : base(message, inner)
    {
    }
}