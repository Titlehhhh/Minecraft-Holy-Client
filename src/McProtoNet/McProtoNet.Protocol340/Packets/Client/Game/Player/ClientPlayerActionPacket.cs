using McProtoNet.Protocol340.Data;
using static McProtoNet.Protocol340.Constans;

namespace McProtoNet.Protocol340.Packets.Client.Game
{


    public sealed class ClientPlayerActionPacket : MinecraftPacket
    {
        public PlayerAction Action { get; set; }
        public Vector3 Position { get; set; }
        public BlockFace Face { get; set; }



        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteVarInt((int)Action);
            long x = (int)Position.X & POSITION_WRITE_SHIFT;
            long y = (int)Position.Y & POSITION_X_SIZE;
            long z = (int)Position.Z & POSITION_WRITE_SHIFT;

            stream.WriteLong(x << POSITION_X_SIZE | y << POSITION_Y_SIZE | z);

            stream.WriteByte((sbyte)Face);
        }

        public ClientPlayerActionPacket(PlayerAction action, Vector3 position, BlockFace face)
        {
            Action = action;
            Position = position;
            Face = face;
        }

        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }


    }
}
