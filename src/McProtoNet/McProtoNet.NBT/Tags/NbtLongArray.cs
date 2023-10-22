using System.Text;

namespace McProtoNet.NBT
{
    /// <summary>
    /// A tag containing an array of signed 64-bit integers. 
    /// </summary>
    public sealed class NbtLongArray : NbtTag
    {
        /// <summary>
        /// Type of this tag (LongArray). 
        /// </summary>
        public override NbtTagType TagType => NbtTagType.LongArray;

        /// <summary>
        /// Value/payload of this tag (an array of signed 64-bit integers). Value is stored as-is and is NOT cloned. May not be <c>null</c>. 
        /// </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is <c>null</c>. </exception>
        public long[] Value
        {
            get => _longs;
            set => _longs = value ?? throw new ArgumentNullException(nameof(value));
        }

        private long[] _longs;

        /// <summary>
        /// Creates an unnamed NbtLongArray tag, containing an empty array of longs. 
        /// </summary>
        public NbtLongArray()
            : this((string)null!) { }

        /// <summary>
        /// Creates an unnamed NbtLongArray tag, containing the given array of longs. 
        /// </summary>
        /// <param name="value"> Long array to assign to this tag's Value. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is <c>null</c>. </exception>
        /// <remarks> Given long array will be cloned. To avoid unnecessary copying, call one of the other constructor
        /// overloads (that do not take a long[]) and then set the Value property yourself. </remarks>
        public NbtLongArray(long[] value)
            : this(null!, value) { }

        /// <summary>
        /// Creates an NbtLongArray tag with the given name, containing an empty array of longs. 
        /// </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        public NbtLongArray(string? tagName)
        {
            Name = tagName;
            _longs = Array.Empty<long>();
        }

        /// <summary>
        /// Creates an NbtLongArray tag with the given name, containing the given array of longs. 
        /// </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        /// <param name="value"> Long array to assign to this tag's Value. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is <c>null</c>. </exception>
        /// <remarks> Given long array will be cloned. To avoid unnecessary copying, call one of the other constructor
        /// overloads (that do not take a long[]) and then set the Value property yourself. </remarks>
        public NbtLongArray(string? tagName, long[] value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            Name = tagName;
            _longs = (long[])value.Clone();
        }

        /// <summary>
        /// Creates a deep copy of given NbtLongArray. 
        /// </summary>
        /// <param name="other"> Tag to copy. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="other"/> is <c>null</c>. </exception>
        /// <remarks> Long array of given tag will be cloned. </remarks>
        public NbtLongArray(NbtLongArray other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            Name = other.Name;
            _longs = (long[])other._longs.Clone();
        }

        /// <summary>
        /// Gets or sets a long at the given index. 
        /// </summary>
        /// <param name="index"> The zero-based index of the element to get or set. </param>
        /// <returns> The long at the specified index. </returns>
        /// <exception cref="IndexOutOfRangeException"> <paramref name="index"/> is outside the array bounds. </exception>
        public new long this[int index]
        {
            get => Value[index];
            set => Value[index] = value;
        }

        internal override bool ReadTag(NbtBinaryReader readStream)
        {
            int length = readStream.ReadInt32();

            if (length < 0)
            {
                throw new NbtFormatException("Negative length given in TAG_Long_Array");
            }

            if (readStream.Selector != null && !readStream.Selector(this))
            {
                readStream.Skip(length * sizeof(long));
                return false;
            }

            Value = new long[length];

            for (int i = 0; i < length; i++)
            {
                Value[i] = readStream.ReadInt64();
            }

            return true;
        }

        internal override void SkipTag(NbtBinaryReader readStream)
        {
            int length = readStream.ReadInt32();

            if (length < 0)
            {
                throw new NbtFormatException("Negative length given in TAG_Long_Array");
            }

            readStream.Skip(length * sizeof(long));
        }

        internal override void WriteTag(NbtBinaryWriter writeStream)
        {
            writeStream.Write(NbtTagType.LongArray);

            if (Name == null)
            {
                throw new NbtFormatException("Name is null");
            }

            writeStream.Write(Name);
            WriteData(writeStream);
        }

        internal override void WriteData(NbtBinaryWriter writeStream)
        {
            writeStream.Write(Value.Length);

            foreach (long t in Value)
            {
                writeStream.Write(t);
            }
        }

        /// <inheritdoc />
        public override object Clone()
        {
            return new NbtLongArray(this);
        }

        internal override void PrettyPrint(StringBuilder sb, string indentString, int indentLevel)
        {
            for (int i = 0; i < indentLevel; i++)
            {
                sb.Append(indentString);
            }

            sb.Append("TAG_Long_Array");

            if (!string.IsNullOrEmpty(Name))
            {
                sb.AppendFormat("(\"{0}\")", Name);
            }

            sb.AppendFormat(": [{0} longs]", Value.Length);
        }
    }
}
