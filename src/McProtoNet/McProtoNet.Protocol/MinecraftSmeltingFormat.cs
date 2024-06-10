namespace McProtoNet.Protocol
{
	public class MinecraftSmeltingFormat
	{
		public string Group { get; private set; }
		public Ingredient Ingredient { get; private set; }
		public Slot Result { get; private set; }
		public float Experience { get; private set; }
		public int CookTime { get; private set; }
	}
}
