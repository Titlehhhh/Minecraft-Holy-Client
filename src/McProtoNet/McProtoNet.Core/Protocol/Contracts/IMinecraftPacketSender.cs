namespace McProtoNet.Core.Protocol
{
	public interface IMinecraftPacketSender : ISwitchCompression
	{
		void SendPacket(Packet packet);
		ValueTask SendPacketAsync(Packet packet, CancellationToken cancellationToken = default);
	}
}

