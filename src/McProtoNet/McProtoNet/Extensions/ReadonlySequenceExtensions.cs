using Cysharp.Text;
using DotNext.Buffers;
using System.Buffers;
using System.Buffers.Binary;
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
		scoped SequenceReader<byte> reader = new SequenceReader<byte>(data);

		reader.TryReadVarInt(out int len, out int read);

		Utf8ValueStringBuilder builder = ZString.CreateUtf8StringBuilder();
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
