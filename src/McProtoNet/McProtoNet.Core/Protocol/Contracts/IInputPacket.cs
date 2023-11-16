using McProtoNet.Core.IO;

namespace McProtoNet.Core.Protocol
{
	public interface IInputPacket
	{
		public abstract void Read(IMinecraftPrimitiveReader stream);
	}
}
