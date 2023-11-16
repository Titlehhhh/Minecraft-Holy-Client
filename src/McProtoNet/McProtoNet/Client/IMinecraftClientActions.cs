using McProtoNet.Core;
using McProtoNet.Core.IO;

namespace McProtoNet
{
	public interface IMinecraftClientActions
	{
		ValueTask SendAction(int type, Vector3 position, BlockFace face);
		ValueTask SendChat(string text);
		ValueTask SendPacket(Action<IMinecraftPrimitiveWriter> action, int id);
		ValueTask SendPacket(Action<IMinecraftPrimitiveWriter> action, PacketOut id);
		ValueTask SendPluginMessage(string channel, byte[] data);
		ValueTask SendPosition(double x, double y, double z, bool onGround);
		ValueTask SendPositionRotation(double x, double y, double z, float yaw, float pitch, bool onGround);
		ValueTask SendRotation(float yaw, float pitch, bool onGround);
		ValueTask SendSettings(string language, byte viewDistance, byte chatMode, bool chatColors, byte skinParts, byte mainHand);
		ValueTask SendTeleportConfirm(int id);
	}
}