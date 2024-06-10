using Cysharp.Text;
using DotNext.Buffers;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace McProtoNet;

public static class ReadonlySequenceExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool TryReadVarInt(this ReadOnlySequence<byte> data, out int value, out int bytesRead)
	{

		scoped SequenceReader<byte> reader = new SequenceReader<byte>(data);

		return reader.TryReadVarInt(out value, out bytesRead);
	}

	public static bool TryReadString(this ReadOnlySequence<byte> data, out string value, out int offset)
	{
		data.TryReadVarInt(out int len, out int read);

		var builder = ZString.CreateUtf8StringBuilder();
		try
		{
			builder.Write(data.Slice(len, read));
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
