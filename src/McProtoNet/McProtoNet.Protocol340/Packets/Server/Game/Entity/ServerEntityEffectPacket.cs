namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerEntityEffectPacket : MinecraftPacket
    {


        //this.entityId = in.readVarInt();
        //this.effect = MagicValues.key(Effect.class, in.readByte());
        //this.amplifier = in.readByte();
        //this.duration = in.readVarInt();
        //
        //int flags = in.readByte();
        //this.ambient = (flags & 0x1) == 0x1;
        //this.showParticles = (flags & 0x2) == 0x2;
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerEntityEffectPacket() { }
    }

}
