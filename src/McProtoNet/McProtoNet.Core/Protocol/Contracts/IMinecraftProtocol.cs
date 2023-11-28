namespace McProtoNet.Core.Protocol
{
	public interface IMinecraftProtocol : IMinecraftPacketSender, IMinecraftPacketReader, IDisposable, IAsyncDisposable, ISwitchCompression
	{
		void SwitchEncryption(byte key);
	}
}

