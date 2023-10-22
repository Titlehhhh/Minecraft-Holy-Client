namespace McProtoNet.Protocol340.Packets.Client.Game
{


    public sealed class ClientWindowActionPacket : MinecraftPacket
    {


        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        //out.writeByte(this.windowId);
        //out.writeShort(this.slot);
        //int param = 0;
        //if(this.action == WindowAction.CLICK_ITEM) {
        //param = MagicValues.value(Integer.class, (Enum<?>) this.param);
        //} else if(this.action == WindowAction.SHIFT_CLICK_ITEM) {
        //param = MagicValues.value(Integer.class, (Enum<?>) this.param);
        //} else if(this.action == WindowAction.MOVE_TO_HOTBAR_SLOT) {
        //param = MagicValues.value(Integer.class, (Enum<?>) this.param);
        //} else if(this.action == WindowAction.CREATIVE_GRAB_MAX_STACK) {
        //param = MagicValues.value(Integer.class, (Enum<?>) this.param);
        //} else if(this.action == WindowAction.DROP_ITEM) {
        //param = MagicValues.value(Integer.class, (Enum<?>) this.param) + (this.slot != -999 ? 2 : 0);
        //} else if(this.action == WindowAction.SPREAD_ITEM) {
        //param = MagicValues.value(Integer.class, (Enum<?>) this.param);
        //} else if(this.action == WindowAction.FILL_STACK) {
        //param = MagicValues.value(Integer.class, (Enum<?>) this.param);
        //}
        //
        //out.writeByte(param);
        //out.writeShort(this.actionId);
        //out.writeByte(MagicValues.value(Integer.class, this.action));
        //NetUtil.writeItem(out, this.clicked);
        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }
    }
}
