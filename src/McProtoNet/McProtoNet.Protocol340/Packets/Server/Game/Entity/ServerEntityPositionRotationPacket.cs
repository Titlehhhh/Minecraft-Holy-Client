namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerEntityPositionRotationPacket : MinecraftPacket
    {
        public int EntityId { get; private set; }
        public double DeltaX { get; private set; }
        public double DeltaY { get; private set; }
        public double DeltaZ { get; private set; }
        public float Yaw { get; private set; }
        public float Pitch { get; private set; }
        public bool OnGround { get; private set; }



        public override void Read(IMinecraftPrimitiveReader stream)
        {
            EntityId = stream.ReadVarInt();
            DeltaX = stream.ReadShort() / 4096D;
            DeltaY = stream.ReadShort() / 4096D;
            DeltaZ = stream.ReadShort() / 4096D;
            Yaw = stream.ReadSignedByte() * 360f / 256f;
            Pitch = stream.ReadSignedByte() * 360f / 256f;
            OnGround = stream.ReadBoolean();
        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerEntityPositionRotationPacket() { }
    }

}
