using System.Text.Json.Serialization;

public sealed class DataPaths
{
	[JsonPropertyName("pc")]
	public Dictionary<string, DataPath> Pc { get; set; }

	[JsonPropertyName("bedrock")]
	public Dictionary<string, DataPath> Bedrock { get; set; }
}
