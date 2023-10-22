namespace McProtoNet.Protocol754.Packets.Server
{


    public sealed class ServerSpawnEntityPacket : MinecraftPacket
    {
        public int EntityId { get; private set; }
        public Guid UUID { get; private set; }
        public int EntityType { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        public float Yaw { get; private set; }
        public float Pitch { get; private set; }
        public int Data { get; private set; }
        public double MotionX { get; private set; }
        public double MotionY { get; private set; }
        public double MotionZ { get; private set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            EntityId = stream.ReadVarInt();
            UUID = stream.ReadUUID();
            EntityType = stream.ReadVarInt();
            X = stream.ReadDouble();
            Y = stream.ReadDouble();
            Z = stream.ReadDouble();

            Pitch = stream.ReadSignedByte() * 360 / 256f;
            Yaw = stream.ReadSignedByte() * 360 / 256f;

            Data = stream.ReadInt();

            MotionX = stream.ReadShort() / 8000D;
            MotionY = stream.ReadShort() / 8000D;
            MotionZ = stream.ReadShort() / 8000D;
        }
        public ServerSpawnEntityPacket() { }
    }
}

