namespace McProtoNet.Protocol756.Packets.Client
{
    public sealed class LoginStartPacket : MinecraftPacket
    {
        public string Nickname { get; private set; }
        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteString(Nickname);
        }

        public override void Read(IMinecraftPrimitiveReader stream)
        {
            Nickname = stream.ReadString();
        }

        public LoginStartPacket(string nickname)
        {
            Nickname = nickname;
        }
        public LoginStartPacket()
        {

        }
    }
}

