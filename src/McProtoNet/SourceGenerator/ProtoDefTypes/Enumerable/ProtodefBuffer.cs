using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefBuffer : ProtodefType
{
    [JsonPropertyName("countType")] public ProtodefType? CountType { get; set; }

    [JsonPropertyName("count")] public object? Count { get; set; }

    [JsonPropertyName("rest")] public bool? Rest { get; set; }

    public override string? GetNetType()
    {
        return "byte[]";
    }

    public override object Clone()
    {
        var owner = new ProtodefBuffer
        {
            CountType = (ProtodefType?)CountType?.Clone() ,
            Count = Count,
            Rest = Rest
        };
        if (owner.CountType is not null) owner.CountType.Parent = owner;
        return owner;
    }
}