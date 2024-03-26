using System.Text.Json.Serialization;

public sealed class VersionInfo
{
	[JsonPropertyName("version")]
	public int Version { get; set; }

	[JsonPropertyName("minecraftVersion")]
	public string MinecraftVersion { get; set; }

	[JsonPropertyName("majorVersion")]
	public string MajorVersion { get; set; }
}
