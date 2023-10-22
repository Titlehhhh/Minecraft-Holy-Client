namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerUnloadChunkPacket : MinecraftPacket
    {
        public int X { get; set; }
        public int Z { get; set; }



        public ServerUnloadChunkPacket()
        {

        }

        public ServerUnloadChunkPacket(int x, int z)
        {
            X = x;
            Z = z;
        }


        //this.x = in.readInt();
        //this.z = in.readInt();
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            X = stream.ReadInt();
            Z = stream.ReadInt();
        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }


    }

}
