using Cysharp.Text;
using DotNext.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

namespace McProtoNet;

public static class BufferWriterSlimExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteVarInt(this ref BufferWriterSlim<byte> writer, int value)
	{
		if (value == 0)
		{
			writer.GetSpan(1)[0] = 0;
			writer.Advance(1);
			return;
		}


		scoped Span<byte> data = writer.GetSpan();

		if (data.Length >= 5)
		{
			byte len = value.GetVarIntLength(data);
			writer.Advance(len);
        }
		else
		{
			data = stackalloc byte[5];
			byte len = value.GetVarIntLength(data);
			writer.Write(data);
		}



	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteString(this ref BufferWriterSlim<byte> writer, string value)
	{
		var builder = ZString.CreateUtf8StringBuilder();
		try
		{
			builder.Append(value);			
			writer.WriteVarInt(builder.Length);
			writer.Write(builder.AsSpan());
		}
		finally
		{
			builder.Dispose();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteBuffer(this ref BufferWriterSlim<byte> writer, ReadOnlySpan<byte> value)
	{
		writer.WriteVarInt(value.Length);
		writer.Write(value);
	}
}
