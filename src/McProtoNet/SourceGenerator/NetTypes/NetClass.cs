using System.Text;

namespace SourceGenerator.NetTypes;

public sealed class NetClass : NetType
{
    public string Modifier { get; set; } = "public";
    public string Name { get; set; }

    public bool IsSealed { get; set; }
    public bool IsPartial { get; set; }

    public string? BaseClass { get; set; }

    public List<NetMethod> Methods { get; set; } = new();

    public List<NetClass> Classes { get; set; } = new();

    public List<NetProperty> Properties { get; set; } = new();

    public List<NetField> Fields { get; set; } = new();

    public List<NetConstructor> Constructors { get; set; } = new();

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.Append(Modifier + " ");
        if (IsSealed)
            stringBuilder.Append("sealed ");
        if (IsPartial)
            stringBuilder.Append("partial ");

        stringBuilder.Append($"class {Name} ");

        if (!string.IsNullOrWhiteSpace(BaseClass))
            stringBuilder.Append($": {BaseClass}");


        stringBuilder.AppendLine("\n{");

        foreach (var item in Constructors)
        {
            item.Name = Name;
            stringBuilder.AppendLine(item.ToString());
        }

        stringBuilder.AppendLine(ToStringTypes(Fields));


        stringBuilder.AppendLine(ToStringTypes(Properties));

        stringBuilder.AppendLine(ToStringTypes(Methods));

        stringBuilder.AppendLine(ToStringTypes(Classes));

        stringBuilder.Append("}");

        return stringBuilder.ToString();
    }

    public sealed class NetConstructor
    {
        public enum ConstructorBaseType
        {
            None,
            This,
            Base
        }

        internal string Name { get; set; }

        public string Modifier { get; set; } = "public";

        public List<(string, string)> Arguments { get; set; } = new();
        public List<string> BaseArguments { get; set; } = new();

        public ConstructorBaseType BaseType { get; set; } = ConstructorBaseType.None;

        public List<string> Instructions { get; set; } = new();

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append(Modifier + " " + Name);
            sb.Append($"({string.Join(", ", Arguments.Select(x => x.Item1 + " " + x.Item2))})");
            if (BaseType == ConstructorBaseType.Base)
                sb.Append(" : base ");
            else if (BaseType == ConstructorBaseType.This) sb.Append(" : this ");

            if (BaseType != ConstructorBaseType.None)
                sb.Append($"({string.Join(", ", Arguments.Select(x => x.Item2))})");

            sb.AppendLine();
            sb.AppendLine("{");

            foreach (var item in Instructions) sb.AppendLine("\t" + item);

            sb.Append("}");
            return sb.ToString();
        }
    }
}