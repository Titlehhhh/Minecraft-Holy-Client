using McProtoNet.NBT;
using System.Buffers.Binary;
using System.Text;

namespace McProtoNet.Core.IO
{
    public class MinecraftPrimitiveWriter : IMinecraftPrimitiveWriter
    {
        public Stream BaseStream { get; set; }
        public MinecraftPrimitiveWriter()
        {

        }
        public MinecraftPrimitiveWriter(Stream stream)
        {
            BaseStream = stream;
        }

        public virtual void WriteUnsignedLong(ulong value)
        {
            Span<byte> span = stackalloc byte[8];
            BinaryPrimitives.WriteUInt64BigEndian(span, value);
            BaseStream.Write(span);
        }

        public virtual void WriteULongArray(ulong[] value)
        {
            WriteVarInt(value.Length);
            byte[] raw = new byte[value.Length * 8];
            Buffer.BlockCopy(value, 0, raw, 0, raw.Length);
            BaseStream.Write(raw);
        }

        public virtual void WriteByte(sbyte value)
        {
            BaseStream.WriteByte((byte)value);
        }

        public virtual void WriteUnsignedByte(byte value)
        {
            BaseStream.WriteByte(value);
        }

        public virtual void WriteBoolean(bool value)
        {
            BaseStream.WriteByte((byte)(value ? 0x01 : 0x00));
        }

        public virtual void WriteUnsignedShort(ushort value)
        {
            // Span<byte> span = stackalloc byte[2];
            // BinaryPrimitives.WriteUInt16BigEndian(span, value);
            // BaseStream.Write(span);
            byte[] theShort = BitConverter.GetBytes(value);
            Array.Reverse(theShort);
            BaseStream.Write(theShort);
        }




        public virtual void WriteShort(short value)
        {
            Span<byte> span = stackalloc byte[2];
            BinaryPrimitives.WriteInt16BigEndian(span, value);
            BaseStream.Write(span);
        }




        public virtual void WriteInt(int value)
        {
            Span<byte> span = stackalloc byte[4];
            BinaryPrimitives.WriteInt32BigEndian(span, value);
            BaseStream.Write(span);
        }




        public virtual void WriteLong(long value)
        {
            Span<byte> span = stackalloc byte[8];
            BinaryPrimitives.WriteInt64BigEndian(span, value);
            BaseStream.Write(span);
        }




        public virtual void WriteFloat(float value)
        {
            Span<byte> span = stackalloc byte[4];
            BinaryPrimitives.WriteSingleBigEndian(span, value);
            BaseStream.Write(span);
        }




        public virtual void WriteDouble(double value)
        {
            Span<byte> span = stackalloc byte[8];
            BinaryPrimitives.WriteDoubleBigEndian(span, value);
            BaseStream.Write(span);
        }




        public virtual void WriteString(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            WriteVarInt(bytes.Length);
            BaseStream.Write(bytes);
        }




        public virtual void WriteVarInt(int value)
        {            
            var unsigned = (uint)value;
            do
            {
                var temp = (byte)(unsigned & 127);
                unsigned >>= 7;

                if (unsigned != 0)
                    temp |= 128;

                BaseStream.WriteByte(temp);
            }
            while (unsigned != 0);
        }

        public virtual void WriteVarInt(Enum value)
        {
            WriteVarInt(Convert.ToInt32(value));
        }

        public virtual void WriteLongArray(long[] values)
        {
            Span<byte> buffer = stackalloc byte[8];
            for (int i = 0; i < values.Length; i++)
            {
                BinaryPrimitives.WriteInt64BigEndian(buffer, values[i]);
                BaseStream.Write(buffer);
            }
        }






        public virtual void WriteVarLong(long value)
        {
            var unsigned = (ulong)value;

            do
            {
                var temp = (byte)(unsigned & 127);

                unsigned >>= 7;

                if (unsigned != 0)
                    temp |= 128;


                BaseStream.WriteByte(temp);
            }
            while (unsigned != 0);
        }






        public virtual void WriteByteArray(byte[] values)
        {
            WriteVarInt(values.Length);
            BaseStream.Write(values);
        }


        public virtual void WriteUuid(Guid value)
        {
            if (value == Guid.Empty)
            {
                WriteLong(0L);
                WriteLong(0L);
            }
            else
            {
                var uuid = System.Numerics.BigInteger.Parse(value.ToString().Replace("-", ""), System.Globalization.NumberStyles.HexNumber);
                Write(uuid.ToByteArray(false, true));
            }
        }

        public virtual void WriteNbt(NbtCompound? nbt, bool root = true)
        {
            string name = "";
            if (root && nbt.HasValue)
                name = nbt.Name;

            var writer = new NbtWriter(BaseStream, name, true);
            if (nbt != null)
            {
                if (root)
                {
                    foreach (var tag in nbt.Tags)
                    {
                        writer.WriteTag(tag);
                    }
                }
                else
                {
                    writer.WriteTag(nbt);
                }
            }
            writer.EndCompound();
            writer.Finish();
        }

        public virtual void Write(byte[] buffer)
        {
            BaseStream.Write(buffer);
        }
    }
}
