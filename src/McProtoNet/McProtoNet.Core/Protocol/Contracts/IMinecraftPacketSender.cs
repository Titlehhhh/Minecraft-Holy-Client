namespace McProtoNet.Core.Protocol
{
	public interface IMinecraftPacketSender : ISwitchCompression, IDisposable
	{
		void SendPacket(Packet packet);
		ValueTask SendPacketAsync(Packet packet, CancellationToken cancellationToken = default);
	}
}

