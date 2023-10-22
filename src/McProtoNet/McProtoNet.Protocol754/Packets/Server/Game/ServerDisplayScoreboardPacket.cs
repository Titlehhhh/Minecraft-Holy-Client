using McProtoNet.Protocol754.Data;

namespace McProtoNet.Protocol754.Packets.Server
{


    public sealed class ServerDisplayScoreboardPacket : MinecraftPacket
    {
        public ScoreboardPosition Position { get; set; }
        public string Name { get; set; }
        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            Position = (ScoreboardPosition)stream.ReadSignedByte();
            Name = stream.ReadString();
        }
        public ServerDisplayScoreboardPacket() { }
    }
}

