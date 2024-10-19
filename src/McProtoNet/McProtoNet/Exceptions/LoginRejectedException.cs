namespace McProtoNet;

public sealed class LoginRejectedException : Exception
{
    public LoginRejectedException(string reason) : base(reason)
    {
    }
}

public sealed class ConfigurationDisconnectException : Exception
{
    public ConfigurationDisconnectException(string reason) : base(reason)
    {
    }
}