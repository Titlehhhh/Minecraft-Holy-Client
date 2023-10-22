namespace McProtoNet.Protocol340.Data
{
    public sealed class EntityMetadata
    {
        public byte Id { get; private set; }
        public MetadataType MetadataType { get; private set; }
        public object Value { get; private set; }

        public EntityMetadata(byte id, MetadataType metadataType, object value)
        {
            Id = id;
            MetadataType = metadataType;
            Value = value;
        }
    }
    public enum MetadataType
    {
        BYTE,
        INT,
        FLOAT,
        STRING,
        CHAT,
        ITEM,
        BOOLEAN,
        ROTATION,
        POSITION,
        OPTIONAL_POSITION,
        BLOCK_FACE,
        OPTIONAL_UUID,
        BLOCK_STATE,
        NBT_TAG
    }
}
