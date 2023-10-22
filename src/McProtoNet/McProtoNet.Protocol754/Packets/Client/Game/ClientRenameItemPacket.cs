namespace McProtoNet.Protocol754.Packets.Client
{


    public sealed class ClientRenameItemPacket : MinecraftPacket
    {
        public string Name { get; private set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteString(Name);
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            Name = stream.ReadString();
        }
        public ClientRenameItemPacket() { }

        public ClientRenameItemPacket(string Name)
        {
            this.Name = Name;
        }
    }
}
