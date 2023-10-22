namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerEntitySetPassengersPacket : MinecraftPacket
    {


        //this.entityId = in.readVarInt();
        //this.passengerIds = new int[in.readVarInt()];
        //for(int index = 0; index < this.passengerIds.length; index++) {
        //this.passengerIds[index] = in.readVarInt();
        //}
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerEntitySetPassengersPacket() { }
    }

}
