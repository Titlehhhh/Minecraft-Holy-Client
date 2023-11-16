namespace McProtoNet
{
	public static class Extensions
	{
		public static ValueTask SendPositionRotation(this IMinecraftClientActions client, Vector3 pos, Rotation rotation, bool onGround)
		{
			return client.SendPositionRotation(pos.X, pos.Y, pos.Z, rotation.Yaw, rotation.Pitch, onGround);
		}
		public static ValueTask SendPosition(this IMinecraftClientActions client, Vector3 pos, bool onGround)
		{
			return client.SendPosition(pos.X, pos.Y, pos.Z, onGround);
		}
		public static ValueTask SendRotation(this IMinecraftClientActions client, Rotation rotation, bool onGround)
		{
			return client.SendRotation(rotation.Yaw, rotation.Pitch, onGround);
		}

	}

}