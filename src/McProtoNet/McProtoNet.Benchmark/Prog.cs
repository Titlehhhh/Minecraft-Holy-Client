using System.Buffers.Binary;

namespace McProtoNet.Benchmark
{
	public class Prog
	{
		public static void Main()
		{
			ushort port = 1243;


			Span<byte> bytes = stackalloc byte[2];

			GetDestinationPortBytes(port, bytes);

			Console.WriteLine(string.Join(", ", bytes.ToArray()));

			Span<byte> aa = stackalloc byte[2];

			GetDestinationPortBytesFast(port, aa);
			Console.WriteLine(string.Join(", ", aa.ToArray()));
			Console.ReadLine();
		}
		internal static void GetDestinationPortBytes(ushort value, Span<byte> buffer)
		{
			buffer[0] = Convert.ToByte(value / 256);
			buffer[1] = Convert.ToByte(value % 256);
		}
		internal static void GetDestinationPortBytesFast(ushort value, Span<byte> buffer)
		{
			BinaryPrimitives.WriteUInt16BigEndian(buffer, value);

		}
	}

}