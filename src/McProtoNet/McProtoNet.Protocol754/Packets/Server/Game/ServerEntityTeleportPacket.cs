namespace McProtoNet.Protocol754.Packets.Server
{


    public sealed class ServerEntityTeleportPacket : MinecraftPacket
    {
        public int EntityId { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        public float Yaw { get; private set; }
        public float Pitch { get; private set; }
        public bool OnGround { get; private set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            EntityId = stream.ReadVarInt();
            X = stream.ReadDouble();
            Y = stream.ReadDouble();
            Z = stream.ReadDouble();
            Yaw = stream.ReadSignedByte() * 256f / 360f;
            Pitch = stream.ReadSignedByte() * 256f / 360f;
            OnGround = stream.ReadBoolean();

        }
        public ServerEntityTeleportPacket() { }
    }
}

