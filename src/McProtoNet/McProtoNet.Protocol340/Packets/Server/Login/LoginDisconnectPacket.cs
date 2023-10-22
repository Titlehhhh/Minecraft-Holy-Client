namespace McProtoNet.Protocol340.Packets.Server
{

    public sealed class LoginDisconnectPacket : MinecraftPacket
    {
        public string Message { get; set; }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            Message = stream.ReadString();
        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteString(Message);
        }

        public LoginDisconnectPacket()
        {

        }

        public LoginDisconnectPacket(string message)
        {
            Message = message;
        }
    }
}
