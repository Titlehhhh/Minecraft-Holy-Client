using McProtoNet.NBT;

namespace McProtoNet.Abstractions
{
	public interface IMinecraftPrimitiveWriter
	{
		void WriteBoolean(bool value);

		void WriteSignedByte(sbyte value);
		void WriteUnsignedByte(byte value);

		void WriteShort(short value);
		void WriteUnsignedShort(ushort value);


		void WriteInt(int value);
		void WriteUnsignedInt(uint value);

		void WriteLong(long value);
		void WriteUnsignedLong(ulong value);

		void WriteFloat(float value);
		void WriteDouble(double value);

		void WriteUUID(Guid value);
		void WriteBuffer(byte[] buffer);

		void WriteVarInt(int value);
		void WriteVarLong(long value);

		void WriteString(string value);

		void WriteNBT(NbtCompound value);
		void WriteOptionalNBT(NbtCompound? value);



	}
}
