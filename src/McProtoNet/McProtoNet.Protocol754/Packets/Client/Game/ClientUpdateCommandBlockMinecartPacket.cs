namespace McProtoNet.Protocol754.Packets.Client
{


    public sealed class ClientUpdateCommandBlockMinecartPacket : MinecraftPacket
    {
        public int EntityId { get; private set; }
        public string Command { get; private set; }
        public bool DoesTrackOutput { get; private set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteVarInt(EntityId);
            stream.WriteString(Command);
            stream.WriteBoolean(DoesTrackOutput);
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            EntityId = stream.ReadVarInt();
            Command = stream.ReadString();
            DoesTrackOutput = stream.ReadBoolean();
        }
        public ClientUpdateCommandBlockMinecartPacket() { }

        public ClientUpdateCommandBlockMinecartPacket(int EntityId, string Command, bool DoesTrackOutput)
        {
            this.EntityId = EntityId;
            this.Command = Command;
            this.DoesTrackOutput = DoesTrackOutput;
        }
    }
}
