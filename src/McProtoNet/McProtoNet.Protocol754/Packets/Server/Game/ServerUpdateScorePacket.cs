using McProtoNet.Protocol754.Data;

namespace McProtoNet.Protocol754.Packets.Server

/* Необъединенное слияние из проекта "McProtoNet.Protocol754 (net6.0)"
До:
{

    
    public sealed class ServerUpdateScorePacket : MinecraftPacket 
После:
{


    public sealed class ServerUpdateScorePacket : MinecraftPacket 
*/
{


    public sealed class ServerUpdateScorePacket : MinecraftPacket
    {
        public string Entry { get; set; }
        public ScoreboardAction Action { get; set; }
        public string Objective { get; set; }
        public int Value { get; set; }
        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            Entry = stream.ReadString();
            Action = (ScoreboardAction)stream.ReadVarInt();
            this.Objective = stream.ReadString();
            if (this.Action == ScoreboardAction.ADD_OR_UPDATE)
            {
                this.Value = stream.ReadVarInt();
            }
        }
        public ServerUpdateScorePacket() { }
    }
}

