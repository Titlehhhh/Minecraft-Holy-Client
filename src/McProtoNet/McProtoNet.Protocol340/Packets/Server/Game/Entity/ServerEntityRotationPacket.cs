namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerEntityRotationPacket : MinecraftPacket
    {
        public int EntityId { get; private set; }

        public float Yaw { get; private set; }
        public float Pitch { get; private set; }
        public bool OnGround { get; private set; }



        public override void Read(IMinecraftPrimitiveReader stream)
        {
            EntityId = stream.ReadVarInt();
            Yaw = stream.ReadSignedByte() * 360f / 256f;
            Pitch = stream.ReadSignedByte() * 360f / 256f;
            OnGround = stream.ReadBoolean();
        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerEntityRotationPacket() { }
    }

}
