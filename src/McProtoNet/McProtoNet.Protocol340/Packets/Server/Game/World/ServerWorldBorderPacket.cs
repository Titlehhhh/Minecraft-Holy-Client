namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerWorldBorderPacket : MinecraftPacket
    {


        //this.action = MagicValues.key(WorldBorderAction.class, in.readVarInt());
        //if(this.action == WorldBorderAction.SET_SIZE) {
        //this.radius = in.readDouble();
        //} else if(this.action == WorldBorderAction.LERP_SIZE) {
        //this.oldRadius = in.readDouble();
        //this.newRadius = in.readDouble();
        //this.speed = in.readVarLong();
        //} else if(this.action == WorldBorderAction.SET_CENTER) {
        //this.centerX = in.readDouble();
        //this.centerY = in.readDouble();
        //} else if(this.action == WorldBorderAction.INITIALIZE) {
        //this.centerX = in.readDouble();
        //this.centerY = in.readDouble();
        //this.oldRadius = in.readDouble();
        //this.newRadius = in.readDouble();
        //this.speed = in.readVarLong();
        //this.portalTeleportBoundary = in.readVarInt();
        //this.warningTime = in.readVarInt();
        //this.warningBlocks = in.readVarInt();
        //} else if(this.action == WorldBorderAction.SET_WARNING_TIME) {
        //this.warningTime = in.readVarInt();
        //} else if(this.action == WorldBorderAction.SET_WARNING_BLOCKS) {
        //this.warningBlocks = in.readVarInt();
        //}
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerWorldBorderPacket() { }
    }

}
