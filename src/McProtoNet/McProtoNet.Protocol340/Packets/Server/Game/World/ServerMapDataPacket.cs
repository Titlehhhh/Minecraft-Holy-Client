namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerMapDataPacket : MinecraftPacket
    {


        //this.mapId = in.readVarInt();
        //this.scale = in.readByte();
        //this.trackingPosition = in.readBoolean();
        //this.icons = new MapIcon[in.readVarInt()];
        //for(int index = 0; index < this.icons.length; index++) {
        //int data = in.readUnsignedByte();
        //int type = (data >> 4) & 15;
        //int rotation = data & 15;
        //int x = in.readUnsignedByte();
        //int z = in.readUnsignedByte();
        //this.icons[index] = new MapIcon(x, z, MagicValues.key(MapIconType.class, type), rotation);
        //}
        //
        //int columns = in.readUnsignedByte();
        //if(columns > 0) {
        //int rows = in.readUnsignedByte();
        //int x = in.readUnsignedByte();
        //int y = in.readUnsignedByte();
        //byte data[] = in.readBytes(in.readVarInt());
        //this.data = new MapData(columns, rows, x, y, data);
        //}
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerMapDataPacket() { }
    }

}
