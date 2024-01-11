namespace McProtoNet.Core.Protocol
{
	public interface IMinecraftPacketReader : ISwitchCompression
	{
		Packet ReadNextPacket();
		ValueTask<Packet> ReadNextPacketAsync(CancellationToken cancellationToken = default);
	}
}

