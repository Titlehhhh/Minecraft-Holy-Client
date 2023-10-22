namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerPreparedCraftingGridPacket : MinecraftPacket
    {


        //this.windowId = in.readByte();
        //this.recipeId = in.readVarInt();
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerPreparedCraftingGridPacket() { }
    }

}
