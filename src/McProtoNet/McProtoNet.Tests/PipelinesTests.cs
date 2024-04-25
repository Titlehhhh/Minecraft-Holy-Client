using LibDeflate;
using McProtoNet.Core.Protocol.Pipelines;
using McProtoNet.Experimental;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace McProtoNet.Tests
{
	[TestClass]
	public class PipelinesTests
	{


		[TestMethod]
		public async Task WriterCancelTest()
		{
			using ZlibCompressor compressor = new(5);

			MinecraftPacketReaderNew reader = new();

			Pipe pipe = new Pipe();


			MinecraftPacketPipeWriter writer = new(pipe.Writer, compressor);

			byte[] data = new byte[100];


			await Assert.ThrowsExceptionAsync<OperationCanceledException>(async () =>
			{
				await writer.SendPacketAsync(data, new CancellationToken(canceled: true));
			});
		}

		[TestMethod]
		public async Task WriterCompleteTest()
		{
			var _pool = new TestMemoryPool();
			var	Pipe = new Pipe(
				new PipeOptions(
					_pool,
					pauseWriterThreshold: 65,
					resumeWriterThreshold: 5,
					readerScheduler: PipeScheduler.Inline,
					writerScheduler: PipeScheduler.Inline,
					useSynchronizationContext: false
				));


			byte[] bytes = "Hello World"u8.ToArray();
			PipeWriter output = Pipe.Writer;
			output.Write(bytes);
			await output.FlushAsync();

			Func<Task> taskFunc = async () => {
				await Task.Delay(1000);

				ReadResult result = await Pipe.Reader.ReadAsync();
				ReadOnlySequence<byte> buffer = result.Buffer;
				Pipe.Reader.AdvanceTo(buffer.End);

				Assert.IsFalse(result.IsCompleted);
				Assert.IsTrue(result.IsCanceled);
				Assert.IsFalse(buffer.IsEmpty);

				output.Write(bytes);
				await output.FlushAsync();

				result = await Pipe.Reader.ReadAsync();
				buffer = result.Buffer;

				Assert.AreEqual(11, buffer.Length);
				Assert.IsTrue(buffer.IsSingleSegment);
				Assert.IsFalse(result.IsCanceled);
				var array = new byte[11];
				buffer.First.Span.CopyTo(array);
				Assert.AreEqual("Hello World", Encoding.ASCII.GetString(array));
				Pipe.Reader.AdvanceTo(result.Buffer.End, result.Buffer.End);

				Pipe.Reader.Complete();
			};
			
			Task task = taskFunc();

			Pipe.Reader.CancelPendingRead();

			await task;

			Pipe.Writer.Complete();

		}
	}
}
