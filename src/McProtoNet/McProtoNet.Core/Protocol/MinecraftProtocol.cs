using System.Net.Sockets;

namespace McProtoNet.Core.Protocol
{

	public sealed class MinecraftProtocol : IMinecraftProtocol
	{

		public MinecraftProtocol(TcpClient tcpClient)
		{

		}

		private Stream _baseStream;

		private IMinecraftPacketReader Reader;
		private IMinecraftPacketSender Sender;

		public MinecraftProtocol(Stream baseStream, bool disposeStream)
		{
			_baseStream = baseStream;
			Reader = new MinecraftPacketReader(_baseStream, disposeStream);
			Sender = new MinecraftPacketSender(_baseStream, disposeStream);
		}

		~MinecraftProtocol()
		{
			Dispose();
		}

		private bool _disposed = false;
		public void Dispose()
		{
			if (_disposed)
				return;
			_disposed = true;

			Reader?.Dispose();
			Sender?.Dispose();
			Reader = null;
			Sender = null;

			GC.SuppressFinalize(this);
		}

		public ValueTask DisposeAsync()
		{
			Dispose();

			return ValueTask.CompletedTask;
		}

		public void SendPacket(Packet packet)
		{
			Sender.SendPacket(packet);
		}

		public ValueTask SendPacketAsync(Packet packet, CancellationToken cancellationToken = default)
		{
			return Sender.SendPacketAsync(packet, cancellationToken);
		}

		public Packet ReadNextPacket()
		{
			return Reader.ReadNextPacket();
		}

		public ValueTask<Packet> ReadNextPacketAsync(CancellationToken cancellationToken = default)
		{
			return Reader.ReadNextPacketAsync(cancellationToken);
		}

		public void SwitchCompression(int threshold)
		{
			Sender.SwitchCompression(threshold);
			Reader.SwitchCompression(threshold);
		}

		public void SwitchEncryption(byte key)
		{

		}
	}


}
