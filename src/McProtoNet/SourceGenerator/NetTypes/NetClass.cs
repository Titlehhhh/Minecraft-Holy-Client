using System.Text;

namespace SourceGenerator.NetTypes
{
	public sealed class NetClass : NetType
	{

		public string Modifier { get; set; } = "public";
		public string Name { get; set; }

		public bool IsSealed { get; set; }
		public bool IsPartial { get; set; }

		public List<NetMethod> Methods { get; set; } = new();

		public List<NetClass> Classes { get; set; } = new();

		public List<NetProperty> Properties { get; set; } = new();


		public override string ToString()
		{
			StringBuilder stringBuilder = new();

			stringBuilder.Append(Modifier);
			if (IsSealed)
				stringBuilder.Append(" sealed ");
			else
				stringBuilder.Append(" ");

			stringBuilder.AppendLine("class " + Name);

			stringBuilder.AppendLine("{");

			stringBuilder.AppendLine(base.ToStringTypes(Properties));

			stringBuilder.AppendLine(base.ToStringTypes(Methods));

			stringBuilder.AppendLine(base.ToStringTypes(Classes));

			stringBuilder.Append("}");

			return stringBuilder.ToString();

		}

	}

}
