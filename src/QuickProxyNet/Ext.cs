using System.Runtime.CompilerServices;

namespace QuickProxyNet
{
	public static class Ext
	{

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static async ValueTask<int> ReadToEndAsync(this Stream stream, Memory<byte> buffer, int length, CancellationToken token)
		{

			int totalRead = 0;
			while (totalRead < length)
			{
				int read = await stream.ReadAsync(buffer.Slice(totalRead), token);
				if (read <= 0)
					throw new EndOfStreamException();

				totalRead += read;
			}

			return totalRead;
		}


	}
}
