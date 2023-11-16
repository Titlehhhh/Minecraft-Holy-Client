namespace McProtoNet.Core.Packets
{
	[AttributeUsage(AttributeTargets.Class)]
	public class PacketInfoAttribute : Attribute
	{
		public int ID { get; private set; }
		public int TargetVersion { get; private set; }
		public PacketCategory Category { get; private set; }
		public PacketSide Side { get; private set; }

		public PacketInfoAttribute(int iD, PacketCategory category, int targetVersion, PacketSide side)
		{
			ID = iD;
			TargetVersion = targetVersion;
			Category = category;
			Side = side;
		}
	}
}
