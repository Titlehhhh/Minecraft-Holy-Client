using McProtoNet.NBT;

namespace McProtoNet.Core.IO
{
	public interface IMinecraftPrimitiveReader
	{

		byte[] ReadToEnd();
		bool ReadBoolean();
		double ReadDouble();
		float ReadFloat();
		Guid ReadUUID();
		int ReadInt();
		long ReadLong();
		short ReadShort();
		/// <summary>
		/// Equalent Java ReadByte();
		/// </summary>
		/// <returns></returns>
		sbyte ReadSignedByte();
		string ReadString(int maxLength = 32767);
		byte[] ReadByteArray();
		byte[] ReadByteArray(int size);
		ulong[] ReadULongArray();
		long[] ReadLongArray();

		byte ReadUnsignedByte();
		ulong ReadUnsignedLong();
		ushort ReadUnsignedShort();
		int ReadVarInt();
		long ReadVarLong();
		/// <summary>
		/// Read optional NBT
		/// </summary>
		/// <returns></returns>
		NbtCompound? ReadOptionalNbt();
	}
}
