using System.Text;

namespace SourceGenerator.NetTypes
{
	public abstract class NetType
	{
		protected string ToStringTypes(IEnumerable<NetType> types)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (var item in types)
			{
				string str = item.ToString();

				str = "\t" + str.Replace(Environment.NewLine, Environment.NewLine + "\t");

				stringBuilder.AppendLine(str);
			}
			return stringBuilder.ToString();
		}
	}

}
