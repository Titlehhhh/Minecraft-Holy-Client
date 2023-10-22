using McProtoNet.Protocol340.Data;

namespace McProtoNet.Protocol340.Packets.Client.Game
{


    public sealed class ClientResourcePackStatusPacket : MinecraftPacket
    {
        public ResourcePackStatus Status { get; private set; }



        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        //out.writeVarInt(MagicValues.value(Integer.class, this.status));
        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteVarInt(Status);
        }
        public ClientResourcePackStatusPacket()
        {

        }

        public ClientResourcePackStatusPacket(ResourcePackStatus status)
        {
            Status = status;
        }
    }
}
