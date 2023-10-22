namespace McProtoNet.Core.Packets
{
    public interface IPacketFactory
    {
        public IPacketProvider CreateProvider(PacketCategory packetCategory, PacketSide side);
    }
}
