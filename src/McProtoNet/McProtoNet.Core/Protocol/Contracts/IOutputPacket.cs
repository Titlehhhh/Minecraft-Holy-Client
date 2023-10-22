using McProtoNet.Core.IO;

namespace McProtoNet.Core.Protocol
{
    public interface IOutputPacket
    {
        public abstract void Write(IMinecraftPrimitiveWriter stream);
    }
}
