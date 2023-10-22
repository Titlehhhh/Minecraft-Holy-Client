namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerPluginMessagePacket : MinecraftPacket
    {
        public string Channel { get; set; }
        public byte[] Data { get; set; }



        public ServerPluginMessagePacket()
        {

        }

        public ServerPluginMessagePacket(string channel, byte[] data)
        {
            Channel = channel;
            Data = data;
        }


        //this.channel = in.readString();
        //this.data = in.readBytes(in.available());
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            Channel = stream.ReadString();
            Data = stream.ReadToEnd();
        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }


    }

}
