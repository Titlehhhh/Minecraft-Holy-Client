using System.Text;

namespace SourceGenerator.NetTypes
{
	public sealed class NetNamespace : NetType
	{
		public string Name { get; set; }

		public List<NetClass> Classes { get; set; } = new();

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();

			stringBuilder.AppendLine($"namespace {Name}")
				.AppendLine("{");

			stringBuilder.AppendLine(base.ToStringTypes(Classes));

			stringBuilder.Append("}");

			return stringBuilder.ToString();
		}
	}

}
