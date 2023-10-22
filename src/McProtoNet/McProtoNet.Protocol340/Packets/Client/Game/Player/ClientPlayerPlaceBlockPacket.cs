namespace McProtoNet.Protocol340.Packets.Client.Game
{


    public sealed class ClientPlayerPlaceBlockPacket : MinecraftPacket
    {


        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        //NetUtil.writePosition(out, this.position);
        //out.writeVarInt(MagicValues.value(Integer.class, this.face));
        //out.writeVarInt(MagicValues.value(Integer.class, this.hand));
        //out.writeFloat(this.cursorX);
        //out.writeFloat(this.cursorY);
        //out.writeFloat(this.cursorZ);
        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }
    }
}
