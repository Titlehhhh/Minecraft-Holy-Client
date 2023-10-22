using System.Text;

namespace McProtoNet.NBT
{
    /// <summary>
    /// A tag containing an array of bytes.
    /// </summary>
    public sealed class NbtByteArray : NbtTag
    {
        /// <summary>
        /// Type of this tag (ByteArray).
        /// </summary>
        public override NbtTagType TagType => NbtTagType.ByteArray;

        /// <summary>
        /// Value/payload of this tag (an array of bytes). Value is stored as-is and is NOT cloned. May not be <c>null</c>.
        /// </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is <c>null</c>. </exception>
        public byte[] Value
        {
            get => _bytes;
            set => _bytes = value ?? throw new ArgumentNullException(nameof(value));
        }

        private byte[] _bytes;

        /// <summary>
        /// Creates an unnamed NbtByte tag, containing an empty array of bytes.
        /// </summary>
        public NbtByteArray()
            : this((string)null!) { }

        /// <summary>
        /// Creates an unnamed NbtByte tag, containing the given array of bytes.
        /// </summary>
        /// <param name="value"> Byte array to assign to this tag's Value. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is <c>null</c>. </exception>
        /// <remarks> Given byte array will be cloned. To avoid unnecessary copying, call one of the other constructor
        /// overloads (that do not take a byte[]) and then set the Value property yourself. </remarks>
        public NbtByteArray(byte[] value)
            : this(null!, value) { }

        /// <summary>
        /// Creates an NbtByte tag with the given name, containing an empty array of bytes.
        /// </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        public NbtByteArray(string? tagName)
        {
            Name = tagName;
            _bytes = Array.Empty<byte>();
        }

        /// <summary>
        /// Creates an NbtByte tag with the given name, containing the given array of bytes.
        /// </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        /// <param name="value"> Byte array to assign to this tag's Value. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is <c>null</c>. </exception>
        /// <remarks> Given byte array will be cloned. To avoid unnecessary copying, call one of the other constructor
        /// overloads (that do not take a byte[]) and then set the Value property yourself. </remarks>
        public NbtByteArray(string? tagName, byte[] value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            Name = tagName;
            _bytes = (byte[])value.Clone();
        }

        /// <summary>
        /// Creates a deep copy of given NbtByteArray.
        /// </summary>
        /// <param name="other"> Tag to copy. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="other"/> is <c>null</c>. </exception>
        /// <remarks> Byte array of given tag will be cloned. </remarks>
        public NbtByteArray(NbtByteArray other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            Name = other.Name;
            _bytes = (byte[])other.Value.Clone();
        }

        /// <summary>
        /// Gets or sets a byte at the given index.
        /// </summary>
        /// <param name="tagIndex"> The zero-based index of the element to get or set. </param>
        /// <returns> The byte at the specified index. </returns>
        /// <exception cref="IndexOutOfRangeException"> <paramref name="tagIndex"/> is outside the array bounds. </exception>
        public new byte this[int tagIndex]
        {
            get => Value[tagIndex];
            set => Value[tagIndex] = value;
        }

        internal override bool ReadTag(NbtBinaryReader readStream)
        {
            int length = readStream.ReadInt32();
            if (length < 0)
            {
                throw new NbtFormatException("Negative length given in TAG_Byte_Array");
            }

            if (readStream.Selector != null && !readStream.Selector(this))
            {
                readStream.Skip(length);
                return false;
            }
            Value = readStream.ReadBytes(length);
            if (Value.Length < length)
            {
                throw new EndOfStreamException();
            }
            return true;
        }

        internal override void SkipTag(NbtBinaryReader readStream)
        {
            int length = readStream.ReadInt32();
            if (length < 0)
            {
                throw new NbtFormatException("Negative length given in TAG_Byte_Array");
            }
            readStream.Skip(length);
        }

        internal override void WriteTag(NbtBinaryWriter writeStream)
        {
            writeStream.Write(NbtTagType.ByteArray);
            if (Name == null) throw new NbtFormatException("Name is null");
            writeStream.Write(Name);
            WriteData(writeStream);
        }

        internal override void WriteData(NbtBinaryWriter writeStream)
        {
            writeStream.Write(Value.Length);
            writeStream.Write(Value, 0, Value.Length);
        }

        /// <inheritdoc />
        public override object Clone()
        {
            return new NbtByteArray(this);
        }

        internal override void PrettyPrint(StringBuilder sb, string indentString, int indentLevel)
        {
            for (int i = 0; i < indentLevel; i++)
            {
                sb.Append(indentString);
            }
            sb.Append("TAG_Byte_Array");
            if (!string.IsNullOrEmpty(Name))
            {
                sb.AppendFormat("(\"{0}\")", Name);
            }
            sb.AppendFormat(": [{0} bytes]", _bytes.Length);
        }
    }
}
