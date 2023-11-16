namespace McProtoNet.Events
{
	public class EntityPositionRotationEventArgs : EventArgs
	{
		public int EntityId { get; internal set; }
		public double DeltaX { get; internal set; }
		public double DeltaY { get; internal set; }
		public double DeltaZ { get; internal set; }

		public byte Yaw { get; internal set; }
		public byte Pitch { get; internal set; }

		public bool OnGround { get; internal set; }

		

		public Vector3 GetVector()
		{
			return new Vector3(DeltaX, DeltaY, DeltaZ);
		}

	}

}
