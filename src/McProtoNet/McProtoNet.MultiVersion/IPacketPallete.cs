namespace McProtoNet.MultiVersion
{
    public interface IPacketPallete
    {
        public int GetOut(PacketOut packet);
        public bool TryGetIn(int id, out PacketIn packetIn);
    }

}