namespace McProtoNet.Protocol340.Packets.Server
{

    public sealed class ServerChatMessagePacket : MinecraftPacket
    {
        public string Message { get; private set; }



        public override void Read(IMinecraftPrimitiveReader stream)
        {
            Message = stream.ReadString();

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }
    }

}
