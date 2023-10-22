using McProtoNet.Protocol756.Data;

namespace McProtoNet.Protocol756.Packets.Client
{


    public sealed class ClientPlayerActionPacket : MinecraftPacket
    {
        public PlayerAction Action { get; set; }

        public Vector3 Position { get; set; }

        public BlockFace Face { get; set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteVarInt(Action);
            stream.WritePosition(Position);
            stream.WriteUnsignedByte((byte)Face);
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }
        public ClientPlayerActionPacket() { }

        public ClientPlayerActionPacket(PlayerAction action, Vector3 position, BlockFace face)
        {
            Action = action;
            Position = position;
            Face = face;
        }
    }
}
