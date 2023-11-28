namespace McProtoNet.Events
{
	public class SpawnEntityEventArgs : EventArgs
	{
		public int Id { get; internal set; }
		public Guid UUID { get; internal set; }

		public int MyProperty { get; internal set; }

	}
}
