namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerSpawnExpOrbPacket : MinecraftPacket
    {
        public int EntityId { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }
        public short Experience { get; private set; }



        //this.entityId = in.readVarInt();
        //this.x = in.readDouble();
        //this.y = in.readDouble();
        //this.z = in.readDouble();
        //this.exp = in.readShort();
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            EntityId = stream.ReadVarInt();
            X = stream.ReadDouble();
            Y = stream.ReadDouble();
            Z = stream.ReadDouble();
            Experience = stream.ReadShort();
        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerSpawnExpOrbPacket() { }
    }

}
