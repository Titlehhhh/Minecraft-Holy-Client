using McProtoNet.Protocol340.Data;

namespace McProtoNet.Protocol340.Packets.Client.Game
{


    public sealed class ClientRequestPacket : MinecraftPacket
    {
        public ClientRequest Request { get; set; }



        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteVarInt((int)Request);
        }

        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public ClientRequestPacket(ClientRequest request)
        {
            Request = request;
        }

        public ClientRequestPacket()
        {

        }

    }
}
