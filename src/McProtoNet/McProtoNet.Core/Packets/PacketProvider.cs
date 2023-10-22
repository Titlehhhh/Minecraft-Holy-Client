

using McProtoNet.Core.Protocol;

namespace McProtoNet.Core.Packets
{
    public class PacketProvider : IPacketProvider
    {
        private readonly Dictionary<Type, int> _outPackets;
        private readonly Dictionary<int, IInputPacket> _inPackets;

        public PacketProvider(Dictionary<Type, int> outPackets, Dictionary<int, IInputPacket> inPackets)
        {
            _outPackets = outPackets;
            _inPackets = inPackets;
        }
        public PacketProvider(Dictionary<int, Type> outPackets, Dictionary<int, Type> inPackets)
        {
            this._outPackets = outPackets.ToDictionary(k => k.Value, v => v.Key);
            this._inPackets = inPackets.ToDictionary(k => k.Key, v => (IInputPacket)Activator.CreateInstance(v.Value));
        }



        public bool TryGetInputPacket(int id, out IInputPacket packet)
        {
            if (_inPackets.TryGetValue(id, out IInputPacket pack))
            {
                packet = pack;
                return true;
            }
            packet = null;
            return false;
        }

        public bool TryGetOutputId(IOutputPacket packet, out int id)
        {
            if (_outPackets.TryGetValue(packet.GetType(), out id))
            {
                return true;
            }
            id = -1;
            return false;
        }


        bool disposed = false;
        public void Dispose()
        {
            if (disposed)
                return;
            disposed = true;
            _outPackets.Clear();
            _inPackets.Clear();
            GC.SuppressFinalize(this);
        }

    }
}
