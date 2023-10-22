namespace McProtoNet.Protocol754.Packets.Client
{


    public sealed class ClientTabCompletePacket : MinecraftPacket
    {
        public int TransactionId { get; private set; }
        public string Text { get; private set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteVarInt(TransactionId);
            stream.WriteString(Text);
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            TransactionId = stream.ReadVarInt();
            Text = stream.ReadString();
        }
        public ClientTabCompletePacket() { }

        public ClientTabCompletePacket(int TransactionId, string Text)
        {
            this.TransactionId = TransactionId;
            this.Text = Text;
        }
    }
}
