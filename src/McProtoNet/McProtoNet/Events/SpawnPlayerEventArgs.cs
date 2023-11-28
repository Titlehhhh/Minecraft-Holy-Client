namespace McProtoNet.Events
{
	public class SpawnPlayerEventArgs : EventArgs
	{
		public int EntityId { get; internal set; }
		public Guid UUID { get; internal set; }
		public double X { get; internal set; }
		public double Y { get; internal set; }
		public double Z { get; internal set; }

		public byte Yaw { get; internal set; }
		public byte Pitch { get; internal set; }



		public Vector3 GetPosition() => new Vector3(X, Y, Z);

	}
}
