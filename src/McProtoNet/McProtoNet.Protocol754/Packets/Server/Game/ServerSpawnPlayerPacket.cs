namespace McProtoNet.Protocol754.Packets.Server
{


    public sealed class ServerSpawnPlayerPacket : MinecraftPacket
    {
        public int EntityId { get; private set; }
        public Guid UUID { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }
        public float Yaw { get; private set; }
        public float Pitch { get; private set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            EntityId = stream.ReadVarInt();
            UUID = stream.ReadUUID();
            X = stream.ReadDouble();
            Y = stream.ReadDouble();
            Z = stream.ReadDouble();
            Yaw = stream.ReadSignedByte() * 360f / 256f;
            Pitch = stream.ReadSignedByte() * 360f / 256f;
        }
        public ServerSpawnPlayerPacket() { }
    }
}

