namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerPlayerHealthPacket : MinecraftPacket
    {
        public float Health { get; private set; }
        public float Food { get; private set; }
        public float Saturation { get; private set; }



        public override void Read(IMinecraftPrimitiveReader stream)
        {
            Health = stream.ReadFloat();
            Food = stream.ReadFloat();
            Saturation = stream.ReadFloat();
        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerPlayerHealthPacket() { }
    }

}
