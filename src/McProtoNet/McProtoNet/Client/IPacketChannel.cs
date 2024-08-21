using McProtoNet.Abstractions;

namespace McProtoNet.Client;

public interface IPacketChannel
{
    IObservable<InputPacket> OnPacket { get; }

    ValueTask SendPacketAsync(OutputPacket packet);
}