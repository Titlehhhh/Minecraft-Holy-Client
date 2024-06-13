using System.Text;

namespace SourceGenerator.NetTypes
{
	public sealed class NetNamespace : NetType
	{
		public string Name { get; set; }

		public List<string> Usings { get; set; } = new();

		public List<NetClass> Classes { get; set; } = new();

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();

			if ((Usings is not null) && Usings.Count > 0)
			{
				foreach (var item in Usings)
				{
					stringBuilder.Append($"using {item};");
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
			}

			stringBuilder.AppendLine($"namespace {Name}")
				.AppendLine("{");

			stringBuilder.AppendLine(base.ToStringTypes(Classes));

			stringBuilder.Append("}");

			return stringBuilder.ToString();
		}
	}

}
