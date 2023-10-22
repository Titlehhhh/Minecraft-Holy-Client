namespace McProtoNet.Protocol754.Packets.Server
{


    public sealed class ServerEntityPositionPacket : MinecraftPacket
    {
        public int EntityId { get; private set; }
        public double DeltaX { get; private set; }
        public double DeltaY { get; private set; }
        public double DeltaZ { get; private set; }
        public bool OnGround { get; private set; }

        public override void Read(IMinecraftPrimitiveReader stream)
        {
            EntityId = stream.ReadVarInt();
            DeltaX = stream.ReadShort() / 4096D;
            DeltaY = stream.ReadShort() / 4096D;
            DeltaZ = stream.ReadShort() / 4096D;
            OnGround = stream.ReadBoolean();
        }
        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerEntityPositionPacket() { }
    }
}

