namespace McProtoNet.Protocol340.Packets.Client.Game
{


    public sealed class ClientSteerVehiclePacket : MinecraftPacket
    {


        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        //out.writeFloat(this.sideways);
        //out.writeFloat(this.forward);
        //byte flags = 0;
        //if(this.jump) {
        //flags = (byte) (flags | 1);
        //}
        //
        //if(this.dismount) {
        //flags = (byte) (flags | 2);
        //}
        //
        //out.writeByte(flags);
        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }
    }
}
