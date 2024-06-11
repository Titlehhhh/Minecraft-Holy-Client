using DotNext;
using System.Buffers;
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
		private Stream baseStream;

		public Task StartAsync(CancellationToken cancellationToken)
		{
			var receive = StartReceiveAsync(cancellationToken);
			var send = StartSendingAsync(cancellationToken);
			return Task.WhenAll(receive, send);
		}
		public void Stop()
		{	
			duplexPipe.Output.Complete();
			duplexPipe.Input.Complete();		
		}

		private async Task StartReceiveAsync(CancellationToken cancellationToken)
		{
			var stream = BaseStream;
			var output = duplexPipe.Output;

			while (!cancellationToken.IsCancellationRequested)
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
					await output.CompleteAsync(ex);
					break;
				}
			}
		}
		private async Task StartSendingAsync(CancellationToken cancellationToken)
		{
			var input = duplexPipe.Input;
			var stream = BaseStream;

			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					ReadResult result = await input.ReadAsync(cancellationToken);

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
				catch
				{

				}
			}
		}



	}
}