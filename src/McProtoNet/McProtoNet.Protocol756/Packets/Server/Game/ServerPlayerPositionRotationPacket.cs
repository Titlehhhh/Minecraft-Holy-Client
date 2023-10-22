namespace McProtoNet.Protocol756.Packets.Server
{


    public sealed class ServerPlayerPositionRotationPacket : MinecraftPacket
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public byte Flags { get; private set; }
        public int TeleportId { get; set; }
        public bool DismountVehicle { get; set; }
        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            X = stream.ReadDouble();
            Y = stream.ReadDouble();
            Z = stream.ReadDouble();

            Yaw = stream.ReadFloat();
            Pitch = stream.ReadFloat();
            Flags = stream.ReadUnsignedByte();
            TeleportId = stream.ReadVarInt();
            DismountVehicle = stream.ReadBoolean();
        }
        public ServerPlayerPositionRotationPacket() { }
        public override string ToString()
        {
            return $"X: {X} Y: {Y} Z:{Z} Yaw:{Yaw} Pitch: {Pitch} Flags:{Flags} TeleportId:{TeleportId} DismountVehicle:{DismountVehicle}";
        }
    }
}

