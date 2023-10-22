namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerBlockBreakAnimPacket : MinecraftPacket
    {


        //this.breakerEntityId = in.readVarInt();
        //this.position = NetUtil.readPosition(in);
        //try {
        //this.stage = MagicValues.key(BlockBreakStage.class, in.readUnsignedByte());
        //} catch(IllegalArgumentException e) {
        //this.stage = BlockBreakStage.RESET;
        //}
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerBlockBreakAnimPacket() { }
    }

}
