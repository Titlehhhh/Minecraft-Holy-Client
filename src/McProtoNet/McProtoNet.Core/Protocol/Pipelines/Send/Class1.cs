using DotNext.Buffers;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McProtoNet.Core.Protocol.Pipelines
{
	public sealed class MinecraftPacketPipeWriter
	{
		private readonly PipeWriter pipeWriter;
		public int CompressionThreshold { get; set; }
		public MinecraftPacketPipeWriter(PipeWriter pipeWriter)
		{
			this.pipeWriter = pipeWriter;
		}

		public ValueTask SendPacketAsync(ReadOnlyMemory<byte> data)
		{
			return ValueTask.CompletedTask;
			if (CompressionThreshold > 0)
			{

			}
		}

	}

}
