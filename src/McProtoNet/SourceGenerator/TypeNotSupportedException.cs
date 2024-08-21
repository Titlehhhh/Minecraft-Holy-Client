public sealed class TypeNotSupportedException : Exception
{
    public TypeNotSupportedException(string type)
    {
        Type = type;
    }

    public string Type { get; set; }
}