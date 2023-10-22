using McProtoNet.Core.IO;
using McProtoNet.Core.Protocol;

namespace McProtoNet.Core
{
    public sealed class StatusPongPacket : MinecraftPacket
    {
        public long PayLoad { get; set; }

        public StatusPongPacket(long payLoad)
        {
            PayLoad = payLoad;
        }
        public StatusPongPacket()
        {

        }

        public override void Read(IMinecraftPrimitiveReader stream)
        {
            PayLoad = stream.ReadLong();
        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteLong(PayLoad);
        }
    }
}
