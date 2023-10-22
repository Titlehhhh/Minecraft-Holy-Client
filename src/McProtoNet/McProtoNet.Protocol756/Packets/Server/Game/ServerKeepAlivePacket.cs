namespace McProtoNet.Protocol756.Packets.Server
{


    public sealed class ServerKeepAlivePacket : MinecraftPacket
    {
        public long PingID { get; set; }
        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteLong(PingID);
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            PingID = stream.ReadLong();
        }
        public ServerKeepAlivePacket() { }
    }
}

