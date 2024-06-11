using DotNext;
using System.Buffers;
using System.Diagnostics;
using System.IO.Pipelines;


namespace McProtoNet.Protocol
{

	internal sealed class TransportHandler : Disposable
	{
		public Stream BaseStream { get; set; }

		private readonly IDuplexPipe duplexPipe;
		public TransportHandler(IDuplexPipe duplexPipe)
		{
			this.duplexPipe = duplexPipe;
		}


		public Task StartAsync(CancellationToken cancellationToken)
		{
			var receive = StartReceiveAsync(cancellationToken);
			var send = StartSendingAsync(cancellationToken);
			return Task.WhenAll(receive, send);
		}
		public void Complete()
		{
			duplexPipe.Output.Complete();
			duplexPipe.Input.Complete();
		}

		private async Task StartReceiveAsync(CancellationToken cancellationToken)
		{
			try
			{

				var stream = BaseStream;
				var output = duplexPipe.Output;

				while (!cancellationToken.IsCancellationRequested)
				{
					try
					{


						try
						{
							Memory<byte> memory = output.GetMemory(4096);
							int bytes = await stream.ReadAsync(memory, cancellationToken);
							output.Advance(bytes);


						}
						catch (OperationCanceledException)
						{
							break;
						}
						catch (Exception ex)
						{
							Debug.WriteLine("Complete 1");
							await output.CompleteAsync(ex);
							break;
						}
						var result = await output.FlushAsync(cancellationToken).ConfigureAwait(false);

						if (result.IsCanceled)
						{
							break;
						}
						if (result.IsCompleted)
						{
							break;
						}
					}
					finally
					{

					}

				}

			}
			finally
			{
				Debug.WriteLine("TransportHandler stop receive");
			}
		}
		private async Task StartSendingAsync(CancellationToken cancellationToken)
		{
			try
			{


				var input = duplexPipe.Input;
				var stream = BaseStream;

				while (!cancellationToken.IsCancellationRequested)
				{

					ReadResult result = await input.ReadAsync(cancellationToken);

					Debug.WriteLine($"Send {result.Buffer.Length} bytes");

					ReadOnlySequence<byte> buffer = result.Buffer;
					try
					{

						foreach (var segment in buffer)
						{
							await stream.WriteAsync(segment, cancellationToken);
						}


						if (result.IsCanceled)
						{
							break;
						}
						if (result.IsCompleted)
						{
							break;
						}
					}
					finally
					{
						input.AdvanceTo(buffer.End);
					}

				}

			}
			finally
			{
				Debug.WriteLine("TransportHandelr stop send");
			}
		}



	}
}