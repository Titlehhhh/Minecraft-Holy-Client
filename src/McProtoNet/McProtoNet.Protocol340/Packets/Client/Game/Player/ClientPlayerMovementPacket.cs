namespace McProtoNet.Protocol340.Packets.Client.Game
{


    public sealed class ClientPlayerMovementPacket : MinecraftPacket
    {
        public bool OnGround { get; set; }



        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteBoolean(OnGround);
        }

        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public ClientPlayerMovementPacket()
        {

        }
        public ClientPlayerMovementPacket(bool onGround)
        {
            OnGround = onGround;
        }
    }
}
