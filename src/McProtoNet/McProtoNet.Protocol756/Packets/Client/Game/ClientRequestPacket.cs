using McProtoNet.Protocol756.Data;

namespace McProtoNet.Protocol756.Packets.Client
{


    public sealed class ClientRequestPacket : MinecraftPacket
    {
        public ClientRequest Request { get; private set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteVarInt(Request);
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }
        public ClientRequestPacket(ClientRequest request)
        {
            Request = request;
        }
        public ClientRequestPacket() { }


    }
}
