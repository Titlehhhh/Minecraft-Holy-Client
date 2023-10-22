namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerPlaySoundPacket : MinecraftPacket
    {


        //String value = in.readString();
        //try {
        //this.sound = MagicValues.key(BuiltinSound.class, value);
        //} catch(IllegalArgumentException e) {
        //this.sound = new CustomSound(value);
        //}
        //
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

        public ServerPlaySoundPacket() { }
    }

}
