using Cysharp.Text;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace McProtoNet;

public static class Utf8ValueStringBuilderExtensions
{
	public static void Write(this ref Utf8ValueStringBuilder builder, in ReadOnlySequence<byte> value)
	{
		if (value.IsSingleSegment)
		{
			builder.Write(value.FirstSpan);
		}
		else
		{
			WriteSlow(ref builder, in value);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		static void WriteSlow(ref Utf8ValueStringBuilder builder, in ReadOnlySequence<byte> value)
		{
			foreach (var segment in value)
				builder.Write(segment.Span);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Write(this ref Utf8ValueStringBuilder builder, ReadOnlySpan<byte> value)
	{
		Span<byte> destination = builder.GetSpan(0);

		// Fast path, try copying to the available memory directly
		if (value.Length <= destination.Length)
		{
			value.CopyTo(destination);
			builder.Advance(value.Length);
		}
		else
		{
			WriteMultiSegment(ref builder, value, destination);
		}
	}

	private static void WriteMultiSegment(this ref Utf8ValueStringBuilder builder, in ReadOnlySpan<byte> source, Span<byte> destination)
	{
		ReadOnlySpan<byte> input = source;
		while (true)
		{
			int writeSize = Math.Min(destination.Length, input.Length);
			input.Slice(0, writeSize).CopyTo(destination);
			builder.Advance(writeSize);
			input = input.Slice(writeSize);
			if (input.Length > 0)
			{
				destination = builder.GetSpan(0);

				if (destination.IsEmpty)
				{
					throw new ArgumentOutOfRangeException();
				}

				continue;
			}

			return;
		}
	}

}
