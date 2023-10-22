namespace McProtoNet.Protocol340.Packets.Client.Game
{


    public sealed class ClientCraftingBookDataPacket : MinecraftPacket
    {


        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        //out.writeVarInt(MagicValues.value(Integer.class, this.type));
        //switch(this.type) {
        //case DISPLAYED_RECIPE:
        //out.writeInt(this.recipeId);
        //break;
        //case CRAFTING_BOOK_STATUS:
        //out.WriteBooleanean(this.craftingBookOpen);
        //out.WriteBooleanean(this.filterActive);
        //break;
        //default:
        //throw new IOException("Unknown crafting book data type: " + this.type);
        //}
        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }
    }
}
