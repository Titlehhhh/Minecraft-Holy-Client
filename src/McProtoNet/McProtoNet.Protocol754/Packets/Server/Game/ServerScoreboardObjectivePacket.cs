using McProtoNet.Protocol754.Data;

namespace McProtoNet.Protocol754.Packets.Server
{


    public sealed class ServerScoreboardObjectivePacket : MinecraftPacket
    {
        public string Name { get; set; }
        public ObjectiveAction Action { get; set; }
        public string? DisplayName { get; set; }
        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            Name = stream.ReadString();
            Action = (ObjectiveAction)stream.ReadSignedByte();
            if (Action == ObjectiveAction.ADD || Action == ObjectiveAction.UPDATE)
            {
                DisplayName = stream.ReadString();
                //TODO
            }
        }
        public ServerScoreboardObjectivePacket() { }
    }
}

