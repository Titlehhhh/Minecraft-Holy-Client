using McProtoNet.Protocol754.Data;

namespace McProtoNet.Protocol754.Packets.Server

/* Необъединенное слияние из проекта "McProtoNet.Protocol754 (net6.0)"
До:
{

    
    public sealed class ServerTitlePacket : MinecraftPacket 
После:
{


    public sealed class ServerTitlePacket : MinecraftPacket 
*/
{


    public sealed class ServerTitlePacket : MinecraftPacket
    {
        public TitleAction Action { get; set; }
        public string Title { get; set; }

        public int FadeIn;
        public int Stay;
        public int FadeOut;
        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            Action = (TitleAction)stream.ReadVarInt();
            switch (this.Action)
            {
                case TitleAction.TITLE:
                case TitleAction.SUBTITLE:
                case TitleAction.ACTION_BAR:
                    this.Title = stream.ReadString();
                    break;
                case TitleAction.TIMES:
                    this.FadeIn = stream.ReadInt();
                    this.Stay = stream.ReadInt();
                    this.FadeOut = stream.ReadInt();
                    break;
                case TitleAction.CLEAR:
                case TitleAction.RESET:
                    break;
            }
        }
        public ServerTitlePacket() { }
    }
}

