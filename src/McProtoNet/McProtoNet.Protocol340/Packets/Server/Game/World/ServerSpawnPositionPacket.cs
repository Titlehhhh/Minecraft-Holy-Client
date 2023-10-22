using McProtoNet.Protocol340.Util;

namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerSpawnPositionPacket : MinecraftPacket
    {
        public Vector3 Position { get; private set; }



        //this.position = NetUtil.readPosition(in);
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            Position = stream.ReadPosition();
        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerSpawnPositionPacket() { }
    }

}
