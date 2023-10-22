namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerPlayBuiltinSoundPacket : MinecraftPacket
    {


        //this.sound = MagicValues.key(BuiltinSound.class, in.readVarInt());
        //this.category = MagicValues.key(SoundCategory.class, in.readVarInt());
        //this.x = in.readInt() / 8D;
        //this.y = in.readInt() / 8D;
        //this.z = in.readInt() / 8D;
        //this.volume = in.readFloat();
        //this.pitch = in.readFloat();
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerPlayBuiltinSoundPacket() { }
    }

}
