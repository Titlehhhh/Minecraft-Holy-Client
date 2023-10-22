namespace McProtoNet.Protocol340.Packets.Client.Game
{


    public sealed class ClientTeleportConfirmPacket : MinecraftPacket
    {
        public int ID { get; set; }



        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteVarInt(ID);
        }

        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public ClientTeleportConfirmPacket(int iD)
        {
            ID = iD;
        }
    }
}
