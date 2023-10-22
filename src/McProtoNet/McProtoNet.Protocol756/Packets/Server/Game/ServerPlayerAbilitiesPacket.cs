namespace McProtoNet.Protocol756.Packets.Server
{
    public sealed class ServerPlayerAbilitiesPacket : MinecraftPacket
    {
        public byte Flags { get; set; }
        public float FlyingSpeed { get; set; }
        public float FieldOfViewModifier { get; set; }

        public ServerPlayerAbilitiesPacket()
        {

        }

        public ServerPlayerAbilitiesPacket(byte flags, float flyingSpeed, float fieldofViewModifier)
        {
            Flags = flags;
            FlyingSpeed = flyingSpeed;
            FieldOfViewModifier = fieldofViewModifier;
        }

        public override void Read(IMinecraftPrimitiveReader stream)
        {
            Flags = stream.ReadUnsignedByte();
            FlyingSpeed = stream.ReadFloat();
            FieldOfViewModifier = stream.ReadFloat();
        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteUnsignedByte(Flags);
            stream.WriteFloat(FlyingSpeed);
            stream.WriteFloat(FieldOfViewModifier);
        }
        public override string ToString()
        {
            return $"Flags: {Flags} FlyingSpeed: {FlyingSpeed} FieldOfViewModifier:{FieldOfViewModifier}";
        }
    }
}

