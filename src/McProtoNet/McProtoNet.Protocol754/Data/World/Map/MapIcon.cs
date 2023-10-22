namespace McProtoNet.Protocol754.Data
{
    public struct MapIcon
    {
        public MapIconType IconType { get; private set; }
        public byte X { get; private set; }
        public byte Z { get; private set; }
        public byte IconRotation { get; private set; }
        public string? DisplayName { get; private set; }

        public MapIcon(MapIconType iconType, byte x, byte z, byte iconRotation, string? displayName = null)
        {
            IconType = iconType;
            X = x;
            Z = z;
            IconRotation = iconRotation;
            DisplayName = displayName;
        }
    }
}
