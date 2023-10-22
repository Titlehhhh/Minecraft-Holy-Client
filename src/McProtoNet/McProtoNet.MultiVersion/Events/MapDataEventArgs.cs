namespace McProtoNet.MultiVersion.Events
{
    public class MapDataEventArgs : EventArgs
    {
        public int MapId { get; }
        public byte Scale { get; }
        public bool TrackingPosition { get; }
        public bool Locked { get; }
        public MapIcon[] Icons { get; }
        public MapData? Data { get; }

        public MapDataEventArgs(int mapId, byte scale, bool trackingPosition, bool locked, MapIcon[] icons, MapData? data)
        {
            MapId = mapId;
            Scale = scale;
            TrackingPosition = trackingPosition;
            Locked = locked;
            Icons = icons;
            Data = data;
        }
    }
}
