namespace McProtoNet.Protocol340.Packets.Client
{
    public sealed class ClientKeepAlivePacket : MinecraftPacket
    {
        public long ID { get; set; }



        //out.writeLong(this.id);
        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteLong(ID);
        }

        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public ClientKeepAlivePacket(long iD)
        {
            ID = iD;
        }
    }
}
