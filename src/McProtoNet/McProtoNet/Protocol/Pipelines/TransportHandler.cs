using DotNext;
using System.Buffers;
using System.IO.Pipelines;


namespace McProtoNet.Protocol
{

	internal sealed class TransportHandler : Disposable
	{
		public Stream BaseStream
		{
			get => baseStream;
			set
			{
				lock (syncRoot)
				{
					baseStream = value;
				}
			}
		}

		private readonly IDuplexPipe duplexPipe;
		public TransportHandler(IDuplexPipe duplexPipe)
		{
			this.duplexPipe = duplexPipe;
		}
		private CancellationTokenSource cts;
		private Stream baseStream;
		private readonly object syncRoot = new();

		private bool _state;
		public Task Start()
		{
			if (_state)
			{
				return Task.CompletedTask;
			}
			completed = false;
			_state = true;
			cts = new CancellationTokenSource();
			var receive = StartReceiveAsync(cts.Token);
			var send = StartSendingAsync(cts.Token);
			return Task.WhenAll(receive, send);
		}
		private bool completed = false;
		public void Stop()
		{
			completed = true;
			cts.Cancel();
			duplexPipe.Output.Complete();
			duplexPipe.Input.Complete();
			cts.Dispose();
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