namespace McProtoNet.Tests;
//[TestClass]
//public class PipelinesTests
//{

//	[TestMethod]
//	public async Task WriterCancelTest()
//	{
//		using ZlibCompressor compressor = new(5);

//		MinecraftPacketReaderNew reader = new();

//		Pipe pipe = new Pipe();

//		MinecraftPacketPipeWriter writer = new(pipe.Writer, compressor);

//		byte[] data = new byte[100];

//		await Assert.ThrowsExceptionAsync<OperationCanceledException>(async () =>
//		{
//			await writer.SendPacketAsync(data, new CancellationToken(canceled: true));
//		});
//	}

//	[TestMethod("Не читать пакеты после отмены")]
//	public async Task WriterCompleteTest()
//	{
//		var pool = new TestMemoryPool();
//		var pipe1 = new Pipe(
//			new PipeOptions(
//				pool,
//				readerScheduler: PipeScheduler.Inline,
//				writerScheduler: PipeScheduler.Inline,
//				useSynchronizationContext: false
//			));

//		var pipe2 = new Pipe(
//			new PipeOptions(
//				pool,
//				readerScheduler: PipeScheduler.Inline,
//				writerScheduler: PipeScheduler.Inline,
//				useSynchronizationContext: false
//			));

//		IDuplexPipe duplex = new DuplexPipe(pipe2.Reader, pipe1.Writer);

//		MinecraftProtocolPipeHandler pipeHandler = new MinecraftProtocolPipeHandler(duplex, 0);

//		bool read = false;

//		byte[] data = new byte[10];
//		Random.Shared.NextBytes(data.AsSpan(1));

//		using MinecraftPacketSenderNew sender = new MinecraftPacketSenderNew();
//		sender.SwitchCompression(0);
//		sender.BaseStream = pipe2.Writer.AsStream(true);
//		for (int i = 0; i < 2; i++)
//		{
//			await sender.SendPacketAsync(new PacketOut(0, 10, data, null));
//		}

//		Task task = Task.Run(async () =>
//		{
//			pipeHandler.OnPacket.Subscribe(p =>
//			{
//				pipe2.Writer.Complete();

//				Assert.IsFalse(read);

//				read = true;

//				CollectionAssert.AreEqual(data.AsSpan(1).ToArray(), p.Data.ToArray());
//			}, onError: (ex) =>
//			{
//				Assert.IsTrue(false, "OnPacket.OnError: " + ex.Message);
//			}, onCompleted: () =>
//			{

//			});

//			await pipeHandler.StartListenAsync();
//		});

//		await task;

//		Assert.IsTrue(read);

//	}

//}