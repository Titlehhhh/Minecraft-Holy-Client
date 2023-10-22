namespace McProtoNet.Protocol340.Packets.Client.Game
{


    public sealed class ClientPlayerStatePacket : MinecraftPacket
    {


        public override void Read(IMinecraftPrimitiveReader stream)
        {
            MemoryStream ms = null;
        }

        //out.writeVarInt(this.entityId);
        //out.writeVarInt(MagicValues.value(Integer.class, this.state));
        //out.writeVarInt(this.jumpBoost);
        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }
    }
}
