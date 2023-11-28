namespace McProtoNet.Events
{
	public class MapDataEventArgs : EventArgs
	{
		public int MapId { get; internal set; }
		public byte Scale { get; internal set; }
		public bool TrackingPosition { get; internal set; }
		public bool Locked { get; internal set; }
		public MapIcon[] Icons { get; internal set; }
		public MapData? Data { get; internal set; } = new();


	}
}
