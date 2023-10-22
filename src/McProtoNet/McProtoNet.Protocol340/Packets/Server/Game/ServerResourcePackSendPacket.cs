namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerResourcePackSendPacket : MinecraftPacket
    {


        //this.url = in.readString();
        //this.hash = in.readString();
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerResourcePackSendPacket() { }
    }

}
