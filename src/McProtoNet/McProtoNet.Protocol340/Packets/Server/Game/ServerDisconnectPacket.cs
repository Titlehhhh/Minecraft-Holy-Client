namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerDisconnectPacket : MinecraftPacket
    {

        public string Message { get; private set; }



        //this.message = Message.fromString(in.readString());
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            Message = stream.ReadString();
        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerDisconnectPacket() { }
    }

}
