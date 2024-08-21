using System.Buffers;
using System.Runtime.CompilerServices;
using Cysharp.Text;

namespace McProtoNet;

public static class ReadonlySequenceExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryReadVarInt(this ReadOnlySequence<byte> data, out int value, out int bytesRead)
    {
        scoped var reader = new SequenceReader<byte>(data);


        return reader.TryReadVarInt(out value, out bytesRead);
    }

    public static bool TryReadString(this ReadOnlySequence<byte> data, out string value, out int offset)
    {
        scoped var reader = new SequenceReader<byte>(data);

        reader.TryReadVarInt(out var len, out var read);

        var builder = ZString.CreateUtf8StringBuilder();
        try
        {
            builder.Write(reader.UnreadSequence);


            value = builder.ToString();
            offset = read + len;
            return true;
        }
        finally
        {
            builder.Dispose();
        }
    }
}