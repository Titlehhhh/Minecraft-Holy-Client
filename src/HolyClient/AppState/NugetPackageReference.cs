using Newtonsoft.Json;

namespace HolyClient.AppState;

[JsonObject("PackageReference")]
public class NugetPackageReference
{
	[JsonProperty("id")]
	public string Id { get; set; }
	[JsonProperty("version")]
	public string Version { get; set; }
}

