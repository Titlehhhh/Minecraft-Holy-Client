namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerAdvancementsPacket : MinecraftPacket
    {


        //this.reset = in.readBoolean();
        //
        //this.advancements = new ArrayList<>();
        //int advancementCount = in.readVarInt();
        //for(int i = 0; i < advancementCount; i++) {
        //String id = in.readString();
        //String parentId = in.readBoolean() ? in.readString() : null;
        //DisplayData displayData = null;
        //if(in.readBoolean()) {
        //Message title = Message.fromString(in.readString());
        //Message description = Message.fromString(in.readString());
        //ItemStack icon = NetUtil.readItem(in);
        //FrameType frameType = MagicValues.key(FrameType.class, in.readVarInt());
        //
        //int flags = in.readInt();
        //boolean hasBackgroundTexture = (flags & 0x1) != 0;
        //boolean showToast = (flags & 0x2) != 0;
        //boolean hidden = (flags & 0x4) != 0;
        //
        //String backgroundTexture = hasBackgroundTexture ? in.readString() : null;
        //float posX = in.readFloat();
        //float posY = in.readFloat();
        //
        //displayData = new DisplayData(title, description, icon, frameType, showToast, hidden, posX, posY, backgroundTexture);
        //}
        //
        //List<String> criteria = new ArrayList<>();
        //int criteriaCount = in.readVarInt();
        //for(int j = 0; j < criteriaCount; j++) {
        //criteria.add(in.readString());
        //}
        //
        //List<List<String>> requirements = new ArrayList<>();
        //int requirementCount = in.readVarInt();
        //for(int j = 0; j < requirementCount; j++) {
        //List<String> requirement = new ArrayList<>();
        //int componentCount = in.readVarInt();
        //for(int k = 0; k < componentCount; k++) {
        //requirement.add(in.readString());
        //}
        //
        //requirements.add(requirement);
        //}
        //
        //this.advancements.add(new Advancement(id, parentId, criteria, requirements, displayData));
        //}
        //
        //this.removedAdvancements = new ArrayList<>();
        //int removedCount = in.readVarInt();
        //for(int i = 0; i < removedCount; i++) {
        //this.removedAdvancements.add(in.readString());
        //}
        //
        //this.progress = new HashMap<>();
        //int progressCount = in.readVarInt();
        //for(int i = 0; i < progressCount; i++) {
        //String advancementId = in.readString();
        //
        //Map<String, Long> advancementProgress = new HashMap<>();
        //int criterionCount = in.readVarInt();
        //for(int j = 0; j < criterionCount; j++) {
        //String criterionId = in.readString();
        //Long achievedDate = in.readBoolean() ? in.readLong() : null;
        //advancementProgress.put(criterionId, achievedDate);
        //}
        //
        //this.progress.put(advancementId, advancementProgress);
        //}
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerAdvancementsPacket() { }
    }

}
