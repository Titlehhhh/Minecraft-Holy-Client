using Humanizer;
using SourceGenerator.MCDataModels;
using SourceGenerator.ProtoDefTypes;
using System.Diagnostics;
using System.Text.Json;


public sealed class ProtocolGenNode
{
	public VersionInfo Version { get; set; }
	public Protocol Protocol { get; set; }
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

		var chain = new List<ProtocolGenNode>();


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

					if (version.Version is >= 340 and <= 765)
					{
						string protocol_json = await File.ReadAllTextAsync(protocol_path);

						ProtodefParser parser = new(protocol_json);

						var protocol = parser.Parse();

						var node = new ProtocolGenNode()
						{
							Protocol = protocol,
							Version = version
						};
						chain.Add(node);
					}
				}
			}
		}



		var ver1_16 = chain.First(x => x.Version.Version == 754);

		await GeneratePackets(ver1_16.Protocol);

	}

	private static async Task GeneratePackets(Protocol protocol)
	{

		ProtocolSourceGenerator generator = new()
		{
			Protocol = protocol,
			Version = "754"
		};

		generator.Generate();




	}


}


