namespace McProtoNet.Protocol754.Packets.Client
{


    public sealed class ClientKeepAlivePacket : MinecraftPacket
    {
        public long PingId { get; private set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteLong(PingId);
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            PingId = stream.ReadLong();
        }
        public ClientKeepAlivePacket() { }

        public ClientKeepAlivePacket(long PingId)
        {
            this.PingId = PingId;
        }
    }
}
