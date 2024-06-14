public sealed class TypeNotSupportedException : Exception
{
    public string Type { get; set; }

    public TypeNotSupportedException(string type)
    {
        Type = type;
    }
}