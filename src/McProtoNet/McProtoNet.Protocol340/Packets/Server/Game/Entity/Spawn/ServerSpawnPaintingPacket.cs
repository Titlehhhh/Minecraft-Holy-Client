namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerSpawnPaintingPacket : MinecraftPacket
    {


        //this.entityId = in.readVarInt();
        //this.uuid = in.readUUID();
        //this.paintingType = MagicValues.key(PaintingType.class, in.readString());
        //this.position = NetUtil.readPosition(in);
        //this.direction = MagicValues.key(HangingDirection.class, in.readUnsignedByte());
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerSpawnPaintingPacket() { }
    }

}
