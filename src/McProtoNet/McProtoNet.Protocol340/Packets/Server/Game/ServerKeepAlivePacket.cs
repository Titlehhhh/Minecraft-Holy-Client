namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerKeepAlivePacket : MinecraftPacket
    {
        public long ID { get; set; }



        public override void Read(IMinecraftPrimitiveReader stream)
        {
            ID = stream.ReadLong();
        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerKeepAlivePacket(long iD)
        {
            ID = iD;
        }

        public ServerKeepAlivePacket() { }
    }

}
