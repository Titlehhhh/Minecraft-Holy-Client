using Humanizer;
using SourceGenerator.MCDataModels;
using SourceGenerator.ProtoDefTypes;
using System.Diagnostics;
using System.Text.Json;


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

	public static string dataPath = @"C:\Users\Title\source\repos\Minecraft-Holy-Client\src\McProtoNet\SourceGenerator\minecraft-data-master\minecraft-data-master\data\";

	private static async Task Main(string[] args)
	{
		var dataPathsJson = Path.Combine(dataPath, "dataPaths.json");

		dataPathsJson = await File.ReadAllTextAsync(dataPathsJson);

		var paths = JsonSerializer.Deserialize<DataPaths>(dataPathsJson);


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


						collection.Add(new Protocol()
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
	private List<Protocol> protocols = new();

	public void Add(Protocol protocol)
	{

	}
}


