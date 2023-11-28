using McProtoNet.Core.IO;
using McProtoNet.Core.Protocol;

namespace McProtoNet.Core
{
	public sealed class StatusResponsePacket : MinecraftPacket
	{
		public string JsonResponse { get; private set; }

		public override void Read(IMinecraftPrimitiveReader stream)
		{
			JsonResponse = stream.ReadString();
		}

		public override void Write(IMinecraftPrimitiveWriter stream)
		{
			stream.WriteString(JsonResponse);
		}
		public StatusResponsePacket()
		{

		}

		public StatusResponsePacket(string jsonResponse)
		{
			JsonResponse = jsonResponse;
		}
	}
}
