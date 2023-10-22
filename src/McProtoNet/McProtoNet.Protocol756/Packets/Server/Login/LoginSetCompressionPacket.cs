namespace McProtoNet.Protocol756.Packets.Server
{
    public sealed class LoginSetCompressionPacket : MinecraftPacket
    {
        public int Threshold { get; set; }

        public override void Read(IMinecraftPrimitiveReader stream)
        {
            Threshold = stream.ReadVarInt();
        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteVarInt(Threshold);
        }

        public LoginSetCompressionPacket(int threshold)
        {
            Threshold = threshold;
        }

        public LoginSetCompressionPacket()
        {

        }
    }
}
