namespace McProtoNet.Protocol340.Packets.Client.Game
{


    public sealed class ClientPlayerPositionRotationPacket : MinecraftPacket
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }

        public bool OnGround { get; set; }



        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteDouble(X);
            stream.WriteDouble(Y);
            stream.WriteDouble(Z);
            stream.WriteFloat(Yaw);
            stream.WriteFloat(Pitch);
            stream.WriteBoolean(OnGround);
        }

        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public ClientPlayerPositionRotationPacket(double x, double y, double z, float yaw, float pitch, bool onGround)
        {
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
            OnGround = onGround;
        }
    }
}
