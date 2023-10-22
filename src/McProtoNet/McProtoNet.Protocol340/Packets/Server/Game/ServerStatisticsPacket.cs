namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerStatisticsPacket : MinecraftPacket
    {


        //int length = in.readVarInt();
        //for(int index = 0; index < length; index++) {
        //String value = in.readString();
        //Statistic statistic = null;
        //if(value.startsWith(CRAFT_ITEM_PREFIX)) {
        //statistic = new CraftItemStatistic(value.substring(CRAFT_ITEM_PREFIX.length()));
        //} else if(value.startsWith(BREAK_BLOCK_PREFIX)) {
        //statistic = new BreakBlockStatistic(value.substring(BREAK_BLOCK_PREFIX.length()));
        //} else if(value.startsWith(USE_ITEM_PREFIX)) {
        //statistic = new UseItemStatistic(value.substring(USE_ITEM_PREFIX.length()));
        //} else if(value.startsWith(BREAK_ITEM_PREFIX)) {
        //statistic = new BreakItemStatistic(value.substring(BREAK_ITEM_PREFIX.length()));
        //} else if(value.startsWith(KILL_ENTITY_PREFIX)) {
        //statistic = new KillEntityStatistic(value.substring(KILL_ENTITY_PREFIX.length()));
        //} else if(value.startsWith(KILLED_BY_ENTITY_PREFIX)) {
        //statistic = new KilledByEntityStatistic(value.substring(KILLED_BY_ENTITY_PREFIX.length()));
        //} else if(value.startsWith(DROP_ITEM_PREFIX)) {
        //statistic = new DropItemStatistic(value.substring(DROP_ITEM_PREFIX.length()));
        //} else if(value.startsWith(PICKUP_ITEM_PREFIX)) {
        //statistic = new PickupItemStatistic(value.substring(PICKUP_ITEM_PREFIX.length()));
        //} else {
        //try {
        //statistic = MagicValues.key(GenericStatistic.class, value);
        //} catch(IllegalArgumentException e) {
        //statistic = new CustomStatistic(value);
        //}
        //}
        //
        //this.statistics.put(statistic, in.readVarInt());
        //}
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerStatisticsPacket() { }
    }

}
