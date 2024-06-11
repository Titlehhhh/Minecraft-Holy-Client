using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

namespace McProtoNet;

public static class SequenceReaderExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool TryReadVarInt(this ref SequenceReader<byte> reader, out int res, out int length)
	{
		int numRead = 0;
		int result = 0;
		byte read;
		do
		{

			if (reader.TryRead(out read))
			{

				int value = read & 127;
				result |= value << 7 * numRead;

				numRead++;
				if (numRead > 5)
				{
					throw new ArithmeticException("VarInt too long");
				}
			}
			else
			{
				res = 0;
				length = -1;
				return false;
			}

		} while ((read & 0b10000000) != 0);



		res = result;
		length = numRead;
		return true;
	}

	public static bool TryReadString(this ref SequenceReader<byte> reader, out string value)
	{
		reader.TryReadVarInt(out int len, out _);

		value = Encoding.UTF8.GetString(reader.UnreadSequence.Slice(0, len).ToArray());

		reader.Advance(len);

		return true;
	}
}
