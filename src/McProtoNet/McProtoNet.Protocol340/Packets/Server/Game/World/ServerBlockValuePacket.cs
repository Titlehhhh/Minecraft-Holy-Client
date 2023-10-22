namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerBlockValuePacket : MinecraftPacket
    {


        //this.position = NetUtil.readPosition(in);
        //int type = in.readUnsignedByte();
        //int value = in.readUnsignedByte();
        //this.blockId = in.readVarInt() & 0xFFF;
        //
        //if(this.blockId == NOTE_BLOCK) {
        //this.type = MagicValues.key(NoteBlockValueType.class, type);
        //this.value = new NoteBlockValue(value);
        //} else if(this.blockId == STICKY_PISTON || this.blockId == PISTON) {
        //this.type = MagicValues.key(PistonValueType.class, type);
        //this.value = MagicValues.key(PistonValue.class, value);
        //} else if(this.blockId == MOB_SPAWNER) {
        //this.type = MagicValues.key(MobSpawnerValueType.class, type);
        //this.value = new MobSpawnerValue();
        //} else if(this.blockId == CHEST || this.blockId == ENDER_CHEST || this.blockId == TRAPPED_CHEST
        //|| (this.blockId >= SHULKER_BOX_LOWER && this.blockId <= SHULKER_BOX_HIGHER)) {
        //this.type = MagicValues.key(ChestValueType.class, type);
        //this.value = new ChestValue(value);
        //} else {
        //this.type = MagicValues.key(GenericBlockValueType.class, type);
        //this.value = new GenericBlockValue(value);
        //}
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerBlockValuePacket() { }
    }

}
