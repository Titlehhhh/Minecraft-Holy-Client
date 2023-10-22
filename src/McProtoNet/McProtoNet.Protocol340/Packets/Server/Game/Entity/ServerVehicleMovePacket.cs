namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerVehicleMovePacket : MinecraftPacket
    {


        //this.x = in.readDouble();
        //this.y = in.readDouble();
        //this.z = in.readDouble();
        //this.yaw = in.readFloat();
        //this.pitch = in.readFloat();
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerVehicleMovePacket() { }
    }

}
