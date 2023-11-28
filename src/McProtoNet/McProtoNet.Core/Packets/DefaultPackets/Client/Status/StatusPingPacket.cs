using McProtoNet.Core.IO;
using McProtoNet.Core.Protocol;

namespace McProtoNet.Core
{
	public sealed class StatusPingPacket : MinecraftPacket
	{
		public long PayLoad { get; set; }

		public StatusPingPacket(long payLoad)
		{
			PayLoad = payLoad;
		}
		public StatusPingPacket()
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