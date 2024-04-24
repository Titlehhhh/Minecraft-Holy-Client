using DotNext.Buffers;
using DotNext.IO.Pipelines;
using LibDeflate;
using Org.BouncyCastle.Bcpg;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace McProtoNet.Core.Protocol.Pipelines
{


	public sealed class MinecraftPacketPipeWriter
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

		public async ValueTask SendPacketAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
		{
			if (CompressionThreshold < 0)
			{
				pipeWriter.WriteVarInt(data.Length);
				pipeWriter.Write(data.Span);
				FlushResult result = await pipeWriter.FlushAsync(cancellationToken).ConfigureAwait(false);

			}
			else
			{
				if (data.Length < CompressionThreshold)
				{
					pipeWriter.WriteVarInt(data.Length + 1);
					pipeWriter.GetSpan(1)[0] = 0;
					pipeWriter.Advance(1);
					FlushResult result = await pipeWriter.FlushAsync(cancellationToken).ConfigureAwait(false);
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

					FlushResult result = await pipeWriter.FlushAsync(cancellationToken).ConfigureAwait(false);


				}
			}
		}

	}

}
