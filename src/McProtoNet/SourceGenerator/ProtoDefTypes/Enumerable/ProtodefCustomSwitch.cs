using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefCustomSwitch : ProtodefSwitch
{
    [JsonIgnore] public string? Owner { get; set; }
}