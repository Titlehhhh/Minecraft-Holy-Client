namespace McProtoNet.Utils;

public sealed class SrvNotFoundException : Exception
{
    public SrvNotFoundException() : base("Srv record not found")
    {
    }
}