using System.Text;

namespace McProtoNet.NBT
{
    /// <summary>
    /// A tag containing a single byte.
    /// </summary>
    public class NbtByte : NbtTag
    {
        /// <summary>
        /// Type of this tag (Byte).
        /// </summary>
        public override NbtTagType TagType => NbtTagType.Byte;

        /// <summary>
        /// Value/payload of this tag (a single byte).
        /// </summary>
        public byte Value { get; set; }

        /// <summary>
        /// Creates an unnamed NbtByte tag with the default value of 0.
        /// </summary>
        public NbtByte() { }

        /// <summary>
        /// Creates an unnamed NbtByte tag with the given value.
        /// </summary>
        /// <param name="value"> Value to assign to this tag. </param>
        public NbtByte(byte value)
            : this(null!, value) { }

        /// <summary>
        /// Creates an NbtByte tag with the given name and the default value of 0.
        /// </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        public NbtByte(string? tagName)
            : this(tagName, 0) { }

        /// <summary>
        /// Creates an NbtByte tag with the given name and value.
        /// </summary>
        /// <param name="tagName"> Name to assign to this tag. May be <c>null</c>. </param>
        /// <param name="value"> Value to assign to this tag. </param>
        public NbtByte(string? tagName, byte value)
        {
            Name = tagName;
            Value = value;
        }

        /// <summary>
        /// Creates a copy of given NbtByte tag.
        /// </summary>
        /// <param name="other"> Tag to copy. May not be <c>null</c>. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="other"/> is <c>null</c>. </exception>
        public NbtByte(NbtByte other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            Name = other.Name;
            Value = other.Value;
        }

        internal override bool ReadTag(NbtBinaryReader readStream)
        {
            if (readStream.Selector != null && !readStream.Selector(this))
            {
                readStream.ReadByte();
                return false;
            }
            Value = readStream.ReadByte();
            return true;
        }

        internal override void SkipTag(NbtBinaryReader readStream)
        {
            readStream.ReadByte();
        }

        internal override void WriteTag(NbtBinaryWriter writeStream)
        {
            writeStream.Write(NbtTagType.Byte);
            if (Name == null) throw new NbtFormatException("Name is null");
            writeStream.Write(Name);
            writeStream.Write(Value);
        }

        internal override void WriteData(NbtBinaryWriter writeStream)
        {
            writeStream.Write(Value);
        }

        /// <inheritdoc />
        public override object Clone()
        {
            return new NbtByte(this);
        }

        internal override void PrettyPrint(StringBuilder sb, string indentString, int indentLevel)
        {
            for (int i = 0; i < indentLevel; i++)
            {
                sb.Append(indentString);
            }
            sb.Append("TAG_Byte");
            if (!string.IsNullOrEmpty(Name))
            {
                sb.AppendFormat("(\"{0}\")", Name);
            }
            sb.Append(": ");
            sb.Append(Value);
        }
    }
}
