namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerEntityEquipmentPacket : MinecraftPacket
    {


        //this.entityId = in.readVarInt();
        //this.slot = MagicValues.key(EquipmentSlot.class, in.readVarInt());
        //this.item = NetUtil.readItem(in);
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerEntityEquipmentPacket() { }
    }

}
