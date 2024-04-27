using DotNext.Buffers;
using DotNext.IO.Pipelines;
using LibDeflate;
using Nerdbank.Streams;
using Org.BouncyCastle.Bcpg;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace McProtoNet.Core.Protocol.Pipelines
{
	internal sealed class MinecraftPacketPipeWriter
	{



		private static readonly byte[] ZeroVarInt = new byte[] { 0 };
		private readonly PipeWriter pipeWriter;
		private readonly ZlibCompressor compressor;
		public int CompressionThreshold { get; set; }
		public MinecraftPacketPipeWriter(PipeWriter pipeWriter, ZlibCompressor compressor)
		{
			this.pipeWriter = pipeWriter;
			this.compressor = compressor;
		}

		public ValueTask<FlushResult> SendPacketAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
		{
			cancellationToken.ThrowIfCancellationRequested();

			if (CompressionThreshold < 0)
			{
				pipeWriter.WriteVarInt(data.Length);
				pipeWriter.Write(data.Span);
				return pipeWriter.FlushAsync(cancellationToken);


			}
			else
			{
				if (data.Length < CompressionThreshold)
				{
					pipeWriter.WriteVarInt(data.Length + 1);
					pipeWriter.WriteVarInt(0);

					return pipeWriter.FlushAsync(cancellationToken);

				}
				else
				{
					int uncompressedSize = data.Length;
					int length = compressor.GetBound(uncompressedSize);

					var compressedBuffer = ArrayPool<byte>.Shared.Rent(length);

					try
					{
						int bytesCompress = compressor.Compress(data.Span, compressedBuffer.AsSpan(0, length));

						int compressedLength = bytesCompress;

						int fullsize = compressedLength + uncompressedSize.GetVarIntLength();

						pipeWriter.WriteVarInt(fullsize);
						pipeWriter.WriteVarInt(uncompressedSize);
						pipeWriter.Write(compressedBuffer.AsSpan(0, bytesCompress));

					}
					finally
					{
						ArrayPool<byte>.Shared.Return(compressedBuffer);
					}

					return pipeWriter.FlushAsync(cancellationToken);
				}
			}

		}

	}

}
