using System.Text;

namespace McProtoNet.NBT
{
	/// <summary>
	/// A byte tag set to either 0 for false and 1 for true
	/// </summary>
	public class NbtBoolean : NbtByte
	{
		public override NbtTagType TagType => NbtTagType.Boolean;

		public bool ValueAsBool
		{
			get => Value != 0;
			private set => Value = value ? (byte)1 : (byte)0;
		}

#pragma warning disable CS0108 // Член скрывает унаследованный член: отсутствует новое ключевое слово
		public byte Value
#pragma warning restore CS0108 // Член скрывает унаследованный член: отсутствует новое ключевое слово
		{
			get => base.Value;
			set => base.Value = value;
		}

		public NbtBoolean()
		{
		}

		public NbtBoolean(bool value) : this(null!, value)
		{ }

		public NbtBoolean(string? tagName) : this(tagName, false)
		{ }

		public NbtBoolean(string? tagName, bool value)
		{
			Name = tagName;
			ValueAsBool = value;
		}

		public NbtBoolean(NbtBoolean other)
		{
			if (other == null) throw new ArgumentNullException(nameof(other));
			Name = other.Name;
			Value = other.Value;
		}

		public NbtBoolean(NbtByte other)
		{
			if (other == null) throw new ArgumentNullException(nameof(other));
			Name = other.Name;
			Value = other.Value;
		}

		internal override bool ReadTag(NbtBinaryReader readStream)
		{
			return base.ReadTag(readStream);
		}

		internal override void SkipTag(NbtBinaryReader readStream)
		{
			readStream.ReadByte();
		}

		internal override void WriteTag(NbtBinaryWriter writeStream)
		{
			base.WriteTag(writeStream);
		}

		internal override void WriteData(NbtBinaryWriter writeStream)
		{
			base.WriteData(writeStream);
		}

		/// <inheritdoc />
		public override object Clone()
		{
			return new NbtBoolean(this);
		}

		internal override void PrettyPrint(StringBuilder sb, string indentString, int indentLevel)
		{
			base.PrettyPrint(sb, indentString, indentLevel);
		}
	}
}
