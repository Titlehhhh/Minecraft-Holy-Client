namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerSpawnMobPacket : MinecraftPacket
    {
        public int EntityId { get; private set; }
        public Guid UUID { get; private set; }
        public int EntityType { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }
        public float Yaw { get; private set; }
        public float Pitch { get; private set; }
        public float HeadYaw { get; private set; }
        public double MotionX { get; private set; }
        public double MotionY { get; private set; }
        public double MotionZ { get; private set; }



        //TODO METADATA

        //this.entityId = in.readVarInt();
        //this.uuid = in.readUUID();
        //this.type = MagicValues.key(MobType.class, in.readVarInt());
        //this.x = in.readDouble();
        //this.y = in.readDouble();
        //this.z = in.readDouble();
        //this.yaw = in.readByte() * 360 / 256f;
        //this.pitch = in.readByte() * 360 / 256f;
        //this.headYaw = in.readByte() * 360 / 256f;
        //this.motX = in.readShort() / 8000D;
        //this.motY = in.readShort() / 8000D;
        //this.motZ = in.readShort() / 8000D;
        //this.metadata = NetUtil.readEntityMetadata(in);
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            EntityId = stream.ReadVarInt();
            EntityType = stream.ReadVarInt();
            X = stream.ReadDouble();
            Y = stream.ReadDouble();
            Z = stream.ReadDouble();
            Yaw = stream.ReadSignedByte() * 360 / 256f;
            Pitch = stream.ReadSignedByte() * 360 / 256f;
            HeadYaw = stream.ReadSignedByte() * 360 / 256f;
            MotionX = stream.ReadShort() / 8000D;
            MotionY = stream.ReadShort() / 8000D;
            MotionZ = stream.ReadShort() / 8000D;
        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerSpawnMobPacket() { }
    }

}
