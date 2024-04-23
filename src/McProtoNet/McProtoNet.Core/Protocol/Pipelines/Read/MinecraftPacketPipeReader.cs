using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Buffers;
using System.Runtime.CompilerServices;
using McProtoNet.Core;
using System.Runtime.InteropServices;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Threading.Channels;
using System.Diagnostics;


namespace McProtoNet.Core.Protocol.Pipelines
{
	public sealed class MinecraftPacketPipeReader
	{


		private readonly PipeReader pipeReader;


		public MinecraftPacketPipeReader(PipeReader pipeReader)
		{
			this.pipeReader = pipeReader;
		}

		public async IAsyncEnumerable<ReadOnlySequence<byte>> ReadPacketsAsync(cancellationToken cancellationToken)
		{
			try
			{

				while (true)
				{
					ReadResult result = await pipeReader.ReadAsync();
					ReadOnlySequence<byte> buffer = result.Buffer;
					//SequencePosition position = buffer.Start;
					while (TryReadPacket(ref buffer, out ReadOnlySequence<byte> packet))
					{
						yield return packet;
						//await processor(packet);
					}

					pipeReader.AdvanceTo(buffer.Start, buffer.End);

					if (result.IsCompleted)
					{
						break;
					}
					if (result.IsCanceled)
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
			scoped SequenceReader<byte> reader = new(buffer);

			packet = ReadOnlySequence<byte>.Empty;

			if (buffer.Length < 1)
			{
				return false; // Недостаточно данных для чтения заголовка пакета
			}

			int length;
			int bytesRead;
			if (!reader.TryReadVarInt(out length, out bytesRead))
			{
				return false; // Невозможно прочитать длину заголовка
			}


			if (length > reader.Remaining)
			{
				return false; // Недостаточно данных для чтения полного пакета
			}

			reader.Advance(length);

			// Чтение данных пакета
			packet = buffer.Slice(bytesRead, length);
			buffer = reader.UnreadSequence;

			return true;
		}



	}

}