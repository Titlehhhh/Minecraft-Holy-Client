namespace McProtoNet.Events
{

	public class PlayerPositionRotationEventArgs : EventArgs
	{
		public double X { get; internal set; }
		public double Y { get; internal set; }
		public double Z { get; internal set; }

		public float Yaw { get; internal set; }
		public float Pitch { get; internal set; }

		public byte Flags { get; internal set; }
		public int TeleportId { get; internal set; }



		public Vector3 GetPosition(Vector3 current)
		{
			byte locMask = Flags;

			current.X = (locMask & 1 << 0) != 0 ? current.X + X : X;
			current.Y = (locMask & 1 << 1) != 0 ? current.Y + Y : Y;
			current.Z = (locMask & 1 << 2) != 0 ? current.Z + Z : Z;
			return current;
		}
		public Vector3 GetPosition()
		{

			return new Vector3(X, Y, Z);
		}
		public Rotation GetRotation()
		{
			return new Rotation(Yaw, Pitch);
		}
	}

}
