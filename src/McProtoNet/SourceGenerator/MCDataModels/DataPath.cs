using System.Text.Json.Serialization;

public sealed class DataPath
{
    [JsonPropertyName("protocol")] public string Protocol { get; set; }

    [JsonPropertyName("version")] public string Version { get; set; }
}