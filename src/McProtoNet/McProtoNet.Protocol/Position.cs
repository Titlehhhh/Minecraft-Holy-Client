namespace McProtoNet.Protocol
{
	public struct Position
	{


		public int X { get; }

		public int Y { get; }

		public int Z { get; }

		public Position(int x, int z, int y)
		{
			X = x;
			Z = z;
			Y = y;
			
		}
	}
}
