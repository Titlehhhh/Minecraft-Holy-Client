using System.Text;

namespace SourceGenerator.NetTypes
{
    public sealed class NetField : NetType
    {
        public string Modifier { get; set; } = "public";

        public string Name { get; set; }

        public bool IsReadOnly { get; set; }

        public string Type { get; set; }

        public string DefaultValue { get; set; }


        public override string ToString()
        {
            StringBuilder sb = new();

            sb.Append(Modifier + " ");
            if (IsReadOnly)
                sb.Append("readonly ");

            sb.Append(Type + " ");
            sb.Append(Name);

            if (!string.IsNullOrWhiteSpace(DefaultValue))
            {
                sb.Append(" = " + DefaultValue);
            }

            sb.Append(";");

            return sb.ToString();
        }
    }
}