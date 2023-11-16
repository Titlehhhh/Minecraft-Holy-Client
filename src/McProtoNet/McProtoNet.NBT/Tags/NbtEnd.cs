using System.Text;

namespace McProtoNet.NBT
{
	internal class NbtEnd : NbtTag
	{
		public override NbtTagType TagType => NbtTagType.End;

		internal override bool ReadTag(NbtBinaryReader readStream)
		{
			return true;
		}

		internal override void SkipTag(NbtBinaryReader readStream)
		{
			readStream.Skip(0);
		}

		internal override void WriteTag(NbtBinaryWriter writeReader)
		{
			writeReader.Write(NbtTagType.End);
		}

		internal override void WriteData(NbtBinaryWriter writeStream)
		{
		}

		public override object Clone()
		{
			return this;
		}

		internal override void PrettyPrint(StringBuilder sb, string indentString, int indentLevel)
		{
		}
	}
}
