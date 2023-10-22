namespace McProtoNet.Protocol754.Packets.Client
{


    public sealed class ClientDisplayedRecipePacket : MinecraftPacket
    {
        public string RecipeId { get; private set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteString(RecipeId);
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            RecipeId = stream.ReadString();
        }
        public ClientDisplayedRecipePacket() { }

        public ClientDisplayedRecipePacket(string RecipeId)
        {
            this.RecipeId = RecipeId;
        }
    }
}
