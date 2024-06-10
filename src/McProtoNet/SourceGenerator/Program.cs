using Humanizer;
using SourceGenerator.MCDataModels;
using SourceGenerator.ProtoDefTypes;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

public sealed class ProtocolVersion
{
	[JsonPropertyName("minecraftVersion")]
	public string MinecraftVersion { get; set; }
	[JsonPropertyName("version")]
	public int Version { get; set; }
	[JsonPropertyName("dataVersion")]
	public int DataVersion { get; set; }
	[JsonPropertyName("usesNetty")]
	public bool UsesNetty { get; set; }
	[JsonPropertyName("majorVersion")]
	public string MajorVersion { get; set; }
	[JsonPropertyName("releaseType")]
	public string ReleaseType { get; set; }
}

public sealed class Protocol
{
	public VersionInfo Version { get; set; }
	public ProtodefProtocol JsonPackets { get; set; }
}

public class Program
{
	public static Dictionary<string, string> MapWriteMethods = new Dictionary<string, string>()
	{
		{"","" }
	};


	public static string Root = "C:\\Users\\Title\\source\\repos\\Minecraft-Holy-Client\\src\\McProtoNet\\McProtoNet.Protocol";

	public static string dataPath = @"minecraft-data\data";

	private static async Task Main(string[] args)
	{
		var dataPathsJson = Path.Combine(dataPath, "dataPaths.json");

		dataPathsJson = await File.ReadAllTextAsync(dataPathsJson);

		var paths = JsonSerializer.Deserialize<DataPaths>(dataPathsJson);


		var allVersions = JsonSerializer.Deserialize<ProtocolVersion[]>(
			await File.ReadAllTextAsync(
				Path.Combine(dataPath, "pc", "common", "protocolVersions.json")));



		var filter = allVersions
			.Where(x => x.Version >= 754)
			.Where(x => x.ReleaseType != "snapshot");

		ProtocolCollection collection = new();

		foreach (var item in paths.Pc)
		{
			string protocol_path = Path.Combine(dataPath, item.Value.Protocol, "protocol.json");
			string version_path = Path.Combine(dataPath, item.Value.Version, "version.json");

			if (File.Exists(version_path))
			{
				if (File.Exists(protocol_path))
				{
					string version_json = await File.ReadAllTextAsync(version_path);


					var version = JsonSerializer.Deserialize<VersionInfo>(version_json);

					if (version.Version >= 754)
					{
						string protocol_json = await File.ReadAllTextAsync(protocol_path);

						ProtodefParser parser = new(protocol_json);

						var protocol = parser.Parse();


						collection.Add(version.Version, new Protocol()
						{
							JsonPackets = protocol,
							Version = version
						});
					}
				}
			}
		}





	}

	private static async Task GeneratePackets(ProtodefProtocol protocol)
	{

		ProtocolSourceGenerator generator = new()
		{
			Protocol = protocol,
			Version = "754"
		};

		generator.Generate();

	}


}

public sealed class ProtocolCollection
{
	public ProtocolCollection()
	{
		Protocols = new();
	}

	public Dictionary<int, Protocol> Protocols { get; }

	public void Add(int version, Protocol protocol)
	{
		if (!Protocols.ContainsKey(version))
		{
			Protocols.Add(version, protocol);
		}
	}
}


