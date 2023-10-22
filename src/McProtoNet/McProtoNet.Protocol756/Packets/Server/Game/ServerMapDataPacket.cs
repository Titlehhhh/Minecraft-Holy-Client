using McProtoNet.Protocol756.Data;
using System.Diagnostics;

namespace McProtoNet.Protocol756.Packets.Server
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
            //throw new AbandonedMutexException("hui");
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {

            this.MapId = stream.ReadVarInt();
            this.Scale = stream.ReadUnsignedByte();

            this.Locked = stream.ReadBoolean();
            this.TrackingPosition = stream.ReadBoolean();
            bool hasIcons = stream.ReadBoolean();
            this.Icons = new MapIcon[hasIcons ? stream.ReadVarInt() : 0];
            if (hasIcons)
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

