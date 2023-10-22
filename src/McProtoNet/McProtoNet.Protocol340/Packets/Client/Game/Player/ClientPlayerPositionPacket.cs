namespace McProtoNet.Protocol340.Packets.Client.Game
{


    public sealed class ClientPlayerPositionPacket : MinecraftPacket
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public bool OnGround { get; set; }



        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteDouble(X);
            stream.WriteDouble(Y);
            stream.WriteDouble(Z);
            stream.WriteBoolean(OnGround);
        }

        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public ClientPlayerPositionPacket(double x, double y, double z, bool onGround)
        {
            X = x;
            Y = y;
            Z = z;
            OnGround = onGround;
        }
    }
}
