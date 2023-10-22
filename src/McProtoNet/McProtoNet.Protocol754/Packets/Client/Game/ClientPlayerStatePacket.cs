using McProtoNet.Protocol754.Data;

namespace McProtoNet.Protocol754.Packets.Client
{


    public sealed class ClientPlayerStatePacket : MinecraftPacket
    {
        public int EntityId { get; set; }
        public PlayerState State { get; set; }
        public int JumpBoost { get; set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteVarInt(EntityId);
            stream.WriteVarInt(State);
            stream.WriteVarInt(JumpBoost);
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }
        public ClientPlayerStatePacket() { }


    }
}
