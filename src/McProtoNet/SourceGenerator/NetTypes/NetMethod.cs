﻿using System.Text;

namespace SourceGenerator.NetTypes;

public sealed class NetMethod : NetType
{
    public string Modifier { get; set; } = "public";

    public string ReturnType { get; set; }


    public bool IsAsync { get; set; } = false;

    public bool IsOverride { get; set; } = false;

    public List<(string, string)> Arguments { get; set; } = new();

    public string Name { get; set; }

    public List<string> Instructions { get; set; } = new();

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        var asy = IsAsync ? " async " : " ";

        var ovr = IsOverride ? " override " : "";

        stringBuilder.AppendLine(
            $"{Modifier}{ovr}{asy}{ReturnType} {Name}({string.Join(", ", Arguments.Select(x => x.Item1 + " " + x.Item2))})");

        stringBuilder.AppendLine("{");

        foreach (var item in Instructions) stringBuilder.AppendLine("\t" + item);

        stringBuilder.Append("}");

        return stringBuilder.ToString();
    }
}