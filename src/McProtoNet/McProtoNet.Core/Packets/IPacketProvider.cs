using McProtoNet.Core.Protocol;

namespace McProtoNet.Core.Packets
{
    public interface IPacketProvider : IDisposable
    {
        bool TryGetInputPacket(int id, out IInputPacket packet);
        bool TryGetOutputId(IOutputPacket Tpacket, out int id);
    }
}
