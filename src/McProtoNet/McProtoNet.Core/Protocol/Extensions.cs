using McProtoNet.Core.IO;
using McProtoNet.Core.Protocol;
using System.Buffers;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;

namespace McProtoNet.Core
{
	public static class Extensions
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryReadVarInt(this ReadOnlySequence<byte> data, out int value, out int bytesRead)
		{

			scoped SequenceReader<byte> reader = new SequenceReader<byte>(data);

			return reader.TryReadVarInt(out value, out bytesRead);


		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ReadVarInt(this Span<byte> data, out int len)
		{



			int numRead = 0;
			int result = 0;
			byte read;
			do
			{

				read = data[numRead];

				int value = read & 0b01111111;
				result |= value << 7 * numRead;

				numRead++;
				if (numRead > 5)
				{
					throw new ArithmeticException("VarInt too long");
				}


			} while ((read & 0b10000000) != 0);

			//data = data.Slice(numRead);


			len = numRead;
			return result;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static byte GetVarIntLength(this int val)
		{

			byte amount = 0;
			do
			{
				val >>= 7;
				amount++;

			} while (val != 0);

			return amount;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static byte GetVarIntLength(this int value, byte[] data)
		{
			return GetVarIntLength(value, data, 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static byte GetVarIntLength(this int value, byte[] data, int offset)
		{
			var unsigned = (uint)value;

			byte len = 0;
			do
			{
				var temp = (byte)(unsigned & 127);
				unsigned >>= 7;

				if (unsigned != 0)
					temp |= 128;

				data[offset + len++] = temp;
			}
			while (unsigned != 0);
			if (len > 5)
				throw new ArithmeticException("Var int is too big");
			return len;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static byte GetVarIntLength(this int value, Span<byte> data)
		{
			var unsigned = (uint)value;

			byte len = 0;



			do
			{
				var temp = (byte)(unsigned & 127);
				unsigned >>= 7;

				if (unsigned != 0)
					temp |= 128;

				data[len++] = temp;
			}
			while (unsigned != 0);
			return len;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static byte GetVarIntLength(this int value, Memory<byte> data)
		{
			return GetVarIntLength(value, data.Span);
		}



		private static int SEGMENT_BITS = 0x7F;
		private static int CONTINUE_BIT = 0x80;
		public static int ReadVarInt(this Stream stream)
		{
			Span<byte> buff = stackalloc byte[1];

			int numRead = 0;
			int result = 0;
			byte read;
			do
			{
				if (stream.Read(buff) <= 0)
				{
					throw new EndOfStreamException();
				}
				read = buff[0];


				int value = read & 0b01111111;
				result |= value << 7 * numRead;

				numRead++;
				if (numRead > 5)
				{
					throw new InvalidOperationException("VarInt is too big");
				}
			} while ((read & 0b10000000) != 0);

			return result;

		}

		public static async ValueTask<int> ReadVarIntAsync(this Stream stream, CancellationToken token = default)
		{

			byte[] buff = ArrayPool<byte>.Shared.Rent(1);
			try
			{
				int numRead = 0;
				int result = 0;
				byte read;
				do
				{
					if (await stream.ReadAsync(buff, 0, 1, token) <= 0)
					{
						throw new EndOfStreamException();
					}
					read = buff[0];


					int value = read & 0b01111111;
					result |= value << 7 * numRead;

					numRead++;
					if (numRead > 5)
					{
						throw new InvalidOperationException("VarInt is too big");
					}
				} while ((read & 0b10000000) != 0);

				return result;
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(buff);
			}
		}



		public static int ReadVarInt(this Stream stream, out int len)
		{
			byte[] buff = new byte[1];

			int numRead = 0;
			int result = 0;
			byte read;
			do
			{

				if (stream.Read(buff, 0, 1) <= 0)
					throw new EndOfStreamException();
				read = buff[0];


				int value = read & 0b01111111;
				result |= value << 7 * numRead;

				numRead++;
				if (numRead > 5)
				{
					throw new InvalidOperationException("VarInt is too big");
				}
			} while ((read & 0b10000000) != 0);
			len = (byte)numRead;
			return result;
		}
		public static void WriteVarInt(this Stream stream, int value)
		{
			var unsigned = (uint)value;

			do
			{
				var temp = (byte)(unsigned & 127);
				unsigned >>= 7;

				if (unsigned != 0)
					temp |= 128;

				stream.WriteByte(temp);

			}
			while (unsigned != 0);
		}
		public static unsafe ValueTask WriteVarIntAsync(this Stream stream, int value, CancellationToken token = default)
		{
			var unsigned = (uint)value;




			var data = ArrayPool<byte>.Shared.Rent(5);
			try
			{
				int len = 0;
				do
				{
					token.ThrowIfCancellationRequested();
					var temp = (byte)(unsigned & 127);
					unsigned >>= 7;

					if (unsigned != 0)
						temp |= 128;
					data[len++] = temp;
				}
				while (unsigned != 0);

				return stream.WriteAsync(data.AsMemory(0, len), token);
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(data);
			}
		}


		public static int ReadToEnd(this Stream stream, Span<byte> buffer, int length)
		{
			int totalRead = 0;
			while (totalRead < length)
			{
				int read = stream.Read(buffer.Slice(totalRead));
				if (read <= 0)
					throw new EndOfStreamException();

				totalRead += read;
			}

			return totalRead;
		}
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



		public static void SendPacket(this IMinecraftPacketSender proto, IOutputPacket pack, int id)
		{

			using (MemoryStream ms = StaticResources.MSmanager.GetStream())
			{
				IMinecraftPrimitiveWriter writer = new MinecraftPrimitiveWriter(ms);
				pack.Write(writer);
				ms.Position = 0;
				proto.SendPacket(new(id, ms));
			}
		}

		public static ValueTask SendPacketAsync(this IMinecraftPacketSender proto, IOutputPacket pack, int id, CancellationToken cancellationToken = default)
		{
			using (MemoryStream ms = StaticResources.MSmanager.GetStream())
			{
				IMinecraftPrimitiveWriter writer = new MinecraftPrimitiveWriter(ms);
				pack.Write(writer);
				ms.Position = 0;
				return proto.SendPacketAsync(new(id, ms), cancellationToken);
			}
		}

		public static async ValueTask CopyToFromMemoryStreamAsync(this MemoryStream source, Stream destination, CancellationToken cancellationToken)
		{
			byte[] buffer = ArrayPool<byte>.Shared.Rent(64);
			try
			{
				int bytesRead;
				while ((bytesRead = await source.ReadAsync(buffer, cancellationToken).ConfigureAwait(false)) != 0)
				{
					await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
				}
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(buffer);
			}
		}
	}
}
