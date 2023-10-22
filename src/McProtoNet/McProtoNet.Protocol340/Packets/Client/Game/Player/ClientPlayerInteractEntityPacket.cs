namespace McProtoNet.Protocol340.Packets.Client.Game
{


    public sealed class ClientPlayerInteractEntityPacket : MinecraftPacket
    {


        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        //out.writeVarInt(this.entityId);
        //out.writeVarInt(MagicValues.value(Integer.class, this.action));
        //if(this.action == InteractAction.INTERACT_AT) {
        //out.writeFloat(this.targetX);
        //out.writeFloat(this.targetY);
        //out.writeFloat(this.targetZ);
        //}
        //
        //if(this.action == InteractAction.INTERACT || this.action == InteractAction.INTERACT_AT) {
        //out.writeVarInt(MagicValues.value(Integer.class, this.hand));
        //}
        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }
    }
}
