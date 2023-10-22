namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerSpawnGlobalEntityPacket : MinecraftPacket
    {
        public int EntityId { get; private set; }
        public sbyte EntityType { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }



        //this.entityId = in.readVarInt();
        //this.type = MagicValues.key(GlobalEntityType.class, in.readByte());
        //this.x = in.readDouble();
        //this.y = in.readDouble();
        //this.z = in.readDouble();
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            EntityId = stream.ReadVarInt();
            EntityType = stream.ReadSignedByte();
            X = stream.ReadDouble();
            Y = stream.ReadDouble();
            Z = stream.ReadDouble();
        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerSpawnGlobalEntityPacket() { }
    }

}
