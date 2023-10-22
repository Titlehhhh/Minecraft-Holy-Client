namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerCombatPacket : MinecraftPacket
    {


        //this.state = MagicValues.key(CombatState.class, in.readVarInt());
        //if(this.state == CombatState.END_COMBAT) {
        //this.duration = in.readVarInt();
        //this.entityId = in.readInt();
        //} else if(this.state == CombatState.ENTITY_DEAD) {
        //this.playerId = in.readVarInt();
        //this.entityId = in.readInt();
        //this.message = Message.fromString(in.readString());
        //}
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerCombatPacket() { }
    }

}
