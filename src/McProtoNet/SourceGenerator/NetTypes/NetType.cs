using System.Text;

namespace SourceGenerator.NetTypes;

public abstract class NetType
{
    protected string ToStringTypes(IEnumerable<NetType> types)
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in types)
        {
            var str = item.ToString();

            str = "\t" + str.Replace(Environment.NewLine, Environment.NewLine + "\t");

            stringBuilder.AppendLine(str);
        }

        return stringBuilder.ToString();
    }
}