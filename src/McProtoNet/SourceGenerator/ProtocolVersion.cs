using System.Text.Json.Serialization;

public sealed class ProtocolVersion
{
    [JsonPropertyName("minecraftVersion")] public string MinecraftVersion { get; set; }

    [JsonPropertyName("version")] public int Version { get; set; }

    [JsonPropertyName("dataVersion")] public int DataVersion { get; set; }

    [JsonPropertyName("usesNetty")] public bool UsesNetty { get; set; }

    [JsonPropertyName("majorVersion")] public string MajorVersion { get; set; }

    [JsonPropertyName("releaseType")] public string ReleaseType { get; set; }
}