using Humanizer;
using SourceGenerator.MCDataModels;
using SourceGenerator.NetTypes;
using SourceGenerator.ProtoDefTypes;
using System;
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
		{ "", "" }
	};


	public static string Testpath = @"C:\Users\Title\OneDrive\Рабочий стол\Test.cs";

	public static string Root =
		"C:\\Users\\Title\\source\\repos\\Minecraft-Holy-Client\\src\\McProtoNet\\McProtoNet.Protocol";

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


		string protocolDir = Path.Combine(Root, "Protocols");

		if (!Directory.Exists(protocolDir))
			Directory.CreateDirectory(protocolDir);



		foreach (var item in collection.Protocols)
		{
			ProtocolSourceGenerator generator = new()
			{
				Protocol = item.Value.JsonPackets,
				Version = item.Key.ToString()
			};

			var ns = generator.Generate();



			string fileName = $"Protocol{item.Key}.cs";

			fileName = Path.Combine(protocolDir, fileName);

			string s = ns.ToString().Replace("\r\n", "\n").Replace("\n", Environment.NewLine);

			await File.WriteAllTextAsync(fileName, s);
		}




	}
}


