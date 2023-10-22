namespace McProtoNet.Protocol340.Packets.Client.Game
{


    public sealed class ClientPlayerRotationPacket : MinecraftPacket
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public bool OnGround { get; set; }



        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteFloat(Yaw);
            stream.WriteFloat(Pitch);
            stream.WriteBoolean(OnGround);
        }

        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public ClientPlayerRotationPacket(float yaw, float pitch, bool onGround)
        {
            Yaw = yaw;
            Pitch = pitch;
            OnGround = onGround;
        }
    }
}
