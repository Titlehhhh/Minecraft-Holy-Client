using McProtoNet.Protocol754.Data;

namespace McProtoNet.Protocol754.Packets.Server
{


    public sealed class ServerMapDataPacket : MinecraftPacket
    {

        public int MapId { get; set; }
        public byte Scale { get; set; }
        public bool TrackingPosition { get; set; }
        public bool Locked { get; set; }
        public MapIcon[] Icons { get; set; }
        public MapData Data { get; set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteVarInt(MapId);
            stream.WriteUnsignedByte(Scale);
            stream.WriteBoolean(TrackingPosition);
            stream.WriteBoolean(Locked);
            stream.WriteVarInt(Icons.Length);
            foreach (var icon in Icons)
            {
                stream.WriteVarInt(icon.IconType);
                stream.WriteUnsignedByte(icon.X);
                stream.WriteUnsignedByte(icon.Z);
                stream.WriteUnsignedByte(icon.IconRotation);
                bool hasName = string.IsNullOrEmpty(icon.DisplayName);
                stream.WriteBoolean(hasName);
                if (hasName)
                {
                    stream.WriteString(icon.DisplayName);
                }
            }
            stream.WriteUnsignedByte(Data.Columns);
            if (Data.Columns > 0)
            {
                stream.WriteUnsignedByte(Data.Rows);
                stream.WriteUnsignedByte(Data.X);
                stream.WriteUnsignedByte(Data.Y);
                stream.WriteByteArray(Data.Data);
            }

        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            this.MapId = stream.ReadVarInt();
            this.Scale = stream.ReadUnsignedByte();
            this.TrackingPosition = stream.ReadBoolean();
            this.Locked = stream.ReadBoolean();
            this.Icons = new MapIcon[stream.ReadVarInt()];
            for (int index = 0; index < this.Icons.Length; index++)
            {
                var type = (MapIconType)stream.ReadVarInt();
                byte x = stream.ReadUnsignedByte();
                byte z = stream.ReadUnsignedByte();
                byte rotation = stream.ReadUnsignedByte();
                string? displayName = null;
                if (stream.ReadBoolean())
                {
                    displayName = stream.ReadString();
                }

                this.Icons[index] = new MapIcon(type, x, z, rotation, displayName);
            }

            byte columns = stream.ReadUnsignedByte();
            if (columns > 0)
            {
                byte rows = stream.ReadUnsignedByte();
                byte x = stream.ReadUnsignedByte();
                byte y = stream.ReadUnsignedByte();
                byte[] data = stream.ReadByteArray();

                this.Data = new MapData(columns, rows, x, y, data);
            }
        }
        public ServerMapDataPacket() { }

        public ServerMapDataPacket(int mapId, byte scale, bool trackingPosition, bool locked, MapIcon[] icons, MapData data)
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

