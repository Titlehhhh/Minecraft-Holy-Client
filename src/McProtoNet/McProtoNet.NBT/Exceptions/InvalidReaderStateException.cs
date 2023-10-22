namespace McProtoNet.NBT
{
    /// <summary>
    /// Exception thrown when an operation is attempted on an NbtReader that
    /// cannot recover from a previous parsing error.
    /// </summary>
    [Serializable]
    public sealed class InvalidReaderStateException : InvalidOperationException
    {
        internal InvalidReaderStateException(string message)
            : base(message) { }
    }
}