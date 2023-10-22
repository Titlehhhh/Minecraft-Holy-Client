namespace McProtoNet.Protocol340.Packets.Client.Game
{

    public sealed class ClientChatPacket : MinecraftPacket
    {


        public string Message { get; set; }
        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteString(Message);
        }

        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public ClientChatPacket(string message)
        {
            Message = message;
        }
    }
}
