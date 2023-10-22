namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerAdvancementTabPacket : MinecraftPacket
    {


        //if(in.readBoolean()) {
        //this.tabId = in.readString();
        //} else {
        //this.tabId = null;
        //}
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerAdvancementTabPacket() { }
    }

}
