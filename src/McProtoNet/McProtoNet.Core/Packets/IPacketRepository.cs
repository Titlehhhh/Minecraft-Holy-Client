namespace McProtoNet.Core.Packets
{
    public interface IPacketRepository : IDisposable
    {
        IPacketProvider GetPackets(PacketCategory category);
    }
}
