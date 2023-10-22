namespace McProtoNet.Core.Protocol
{
    public interface IPacketReaderWriter : IDisposable
    {
        void SwitchEncryption(byte[] privateKey);
        void SwitchCompression(int threshold);
        public Task<MinecraftPacket> ReadNextPacketAsync(CancellationToken cancellationToken = default);
        public MinecraftPacket ReadNextPacket();
        public Task SendPacketAsync(MinecraftPacket packet, CancellationToken cancellationToken = default);
        public void SendPacket(MinecraftPacket packet);
    }

}

