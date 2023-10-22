namespace McProtoNet.Core.Packets
{
    public class PacketRepository : IPacketRepository
    {
        public Dictionary<PacketCategory, IPacketProvider> AllPAckets { get; set; }

        public PacketRepository(Dictionary<PacketCategory, IPacketProvider> allPAckets)
        {
            AllPAckets = allPAckets;
        }
        private bool _disposed = false;
        public void Dispose()
        {
            if (_disposed)
                return;
            AllPAckets.Clear();
            AllPAckets = null;
            _disposed = true;
            GC.SuppressFinalize(this);

        }

        public IPacketProvider GetPackets(PacketCategory category)
        {
            return AllPAckets[category];
        }
    }
}
