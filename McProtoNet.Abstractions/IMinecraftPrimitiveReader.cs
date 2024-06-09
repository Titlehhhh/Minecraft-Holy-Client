using McProtoNet.NBT;

namespace McProtoNet.Serialization
{

	public interface IMinecraftPrimitiveReader
	{

		bool ReadBoolean();

		sbyte ReadSignedByte();
		byte ReadUnsignedByte();

		short ReadShort();
		ushort ReadUnsignedShort();

		int ReadInt();
		uint ReadUnsignedInt();

		long ReadLong();
		ulong ReadUnsignedLong();

		float ReadFloat();
		double ReadDouble();


		int ReadVarInt();
		long ReadVarLong();

		Guid ReadUUID();

		string ReadString(int maxLength = 32767);


		byte[] ReadBuffer();

		byte[] ReadRestBuffer();


		NbtCompound? ReadOptionalNbt();
		NbtCompound ReadNbt();
	}
}
