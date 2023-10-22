namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerConfirmTransactionPacket : MinecraftPacket
    {


        //this.windowId = in.readUnsignedByte();
        //this.actionId = in.readShort();
        //this.accepted = in.readBoolean();
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerConfirmTransactionPacket() { }
    }

}
