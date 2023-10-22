namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerPlayerListDataPacket : MinecraftPacket
    {


        //this.header = Message.fromString(in.readString());
        //this.footer = Message.fromString(in.readString());
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerPlayerListDataPacket() { }
    }

}
