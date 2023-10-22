using McProtoNet.Protocol340.Data;

namespace McProtoNet.Protocol340.Packets.Client.Game
{


    public sealed class ClientSettingsPacket : MinecraftPacket
    {
        public string Locale { get; set; }
        public byte RenderDistance { get; set; }
        public ChatVisibility ChatVisibility { get; set; }
        public bool UseChatColors { get; set; }
        public List<SkinPart> VisibleParts { get; set; }
        public HandPreference MainHand { get; set; }



        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        //out.writeString(this.locale);
        //out.writeByte(this.renderDistance);
        //out.writeVarInt(MagicValues.value(Integer.class, this.chatVisibility));
        //out.WriteBooleanean(this.chatColors);
        //
        //int flags = 0;
        //for(SkinPart part : this.visibleParts) {
        //flags |= 1 << part.ordinal();
        //}
        //
        //out.writeByte(flags);
        //
        //out.writeVarInt(MagicValues.value(Integer.class, this.mainHand));
        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteString(Locale);
            stream.WriteUnsignedByte(RenderDistance);
            stream.WriteVarInt(ChatVisibility);
            stream.WriteBoolean(UseChatColors);

            int flags = 0;
            foreach (SkinPart part in VisibleParts)
            {
                flags |= 1 << ((int)part);
            }

            stream.WriteUnsignedByte((byte)flags);

            stream.WriteVarInt(MainHand);
        }
        public ClientSettingsPacket()
        {

        }
        public ClientSettingsPacket(string locale, byte renderDistance, ChatVisibility chatVisibility, bool useChatColors, List<SkinPart> visibleParts, HandPreference mainHand)
        {
            Locale = locale;
            RenderDistance = renderDistance;
            ChatVisibility = chatVisibility;
            UseChatColors = useChatColors;
            VisibleParts = visibleParts;
            MainHand = mainHand;
        }
    }
}
