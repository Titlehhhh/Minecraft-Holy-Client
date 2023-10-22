namespace McProtoNet.Core.Packets
{
    public class UnRegisterPacketEventArgs : EventArgs
    {
        public IList<Type> Packets { get; private set; }

        public UnRegisterPacketEventArgs(IList<Type> packets)
        {
            Packets = packets;
        }
    }
}
