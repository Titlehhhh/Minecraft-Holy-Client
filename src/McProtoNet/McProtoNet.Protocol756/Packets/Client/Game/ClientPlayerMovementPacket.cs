namespace McProtoNet.Protocol756.Packets.Client
{
    public sealed class ClientPlayerMovementPacket : MinecraftPacket
    {       
        public bool OnGround { get; private set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {
           
            stream.WriteBoolean(OnGround);
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
           
            OnGround = stream.ReadBoolean();
        }
        public ClientPlayerMovementPacket() { }

        public ClientPlayerMovementPacket( bool OnGround)
        {            
            this.OnGround = OnGround;
        }
        public override string ToString()
        {
            return $" OnGround:{OnGround}";
        }
    }
}
