using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Buffers;
using System.Runtime.CompilerServices;
using DotNext.Buffers;
using McProtoNet.Core;
using System.Runtime.InteropServices;
using LibDeflate;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Threading.Channels;
using System.Diagnostics;


namespace McProtoNet.Core.Protocol.Pipelines
{



	public sealed class PacketPipeReader
	{


		private readonly PipeReader pipeReader;


		public PacketPipeReader(PipeReader pipeReader)
		{
			this.pipeReader = pipeReader;
		}

		public async IAsyncEnumerable<ReadOnlySequence<byte>> ReadPacketsAsync()
		{
			try
			{
				while (true)
				{
					ReadResult result = await pipeReader.ReadAsync();
					ReadOnlySequence<byte> buffer = result.Buffer;

					while (TryReadPacket(ref buffer, out ReadOnlySequence<byte> packet))
					{
						yield return packet;
					}

					pipeReader.AdvanceTo(buffer.Start, buffer.End);

					if (result.IsCompleted)
					{
						break;
					}
				}
			}
			finally
			{
				await pipeReader.CompleteAsync();
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool TryReadPacket(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> packet)
		{
			packet = default;

			if (buffer.Length < 1)
			{
				return false; // Недостаточно данных для чтения заголовка пакета
			}

			int length;
			int bytesRead;
			if (!TryReadVarInt(buffer, out length, out bytesRead))
			{
				return false; // Невозможно прочитать длину заголовка
			}


			if ((length + bytesRead) < buffer.Length)
			{
				return false; // Недостаточно данных для чтения полного пакета
			}


			// Чтение данных пакета
			packet = buffer.Slice(bytesRead, length);

			buffer = buffer.Slice(bytesRead + length);

			return true;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ReadVarInt(Span<byte> data, out int len)
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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryReadVarInt(ReadOnlySequence<byte> buffer, out int value, out int bytesRead)
		{
			value = 0;
			bytesRead = 0;
			int shift = 0;

			foreach (var segment in buffer)
			{
				foreach (var b in segment.Span)
				{
					value |= (b & 127) << shift;
					shift += 7;
					bytesRead++;

					if ((b & 0x80) == 0)
					{

						return true;
					}

					if (bytesRead >= 5)
					{
						throw new ArithmeticException("Varint is big");
					}
				}
			}

			return false; // Недостаточно данных для чтения varint
		}

		public static bool TryReadVarInt(ref SequenceReader<byte> reader, out int res, out int length)
		{



			int numRead = 0;
			int result = 0;
			byte read;
			do
			{

				if (reader.TryPeek(numRead, out read))
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

			reader.Advance(numRead);

			res = result;
			length = numRead;
			return true;
		}

	}

}