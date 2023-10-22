namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerEntityDestroyPacket : MinecraftPacket
    {
        public int[] EntityIds { get; private set; }



        //this.entityIds = new int[in.readVarInt()];
        //for(int index = 0; index < this.entityIds.length; index++) {
        //this.entityIds[index] = in.readVarInt();
        //}
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            EntityIds = new int[stream.ReadVarInt()];
            for (int i = 0; i < EntityIds.Length; i++)
            {
                EntityIds[i] = stream.ReadVarInt();
            }

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerEntityDestroyPacket() { }
    }

}
