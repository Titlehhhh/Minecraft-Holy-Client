using McProtoNet.Protocol754.Data;

namespace McProtoNet.Protocol754.Packets.Client
{


    public sealed class ClientPlayerInteractEntityPacket : MinecraftPacket
    {
        public int EntityId { get; set; }
        public InteractEntity Interact { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public Hand Hand { get; set; }
        public bool IsSneaking { get; set; }
        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteVarInt(EntityId);
            stream.WriteVarInt(Interact);
            if (Interact == InteractEntity.InteractAt)
            {
                stream.WriteFloat(X);
                stream.WriteFloat(Y);
                stream.WriteFloat(Z);
            }
            if (Interact != InteractEntity.Attack)
            {
                stream.WriteVarInt(Hand);
            }
            stream.WriteBoolean(IsSneaking);
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }
        public ClientPlayerInteractEntityPacket() { }


    }
}
