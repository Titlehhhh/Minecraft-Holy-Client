﻿using SourceGenerator.ProtoDefTypes;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;


public sealed class ProtocolGenNode
{
	public VersionInfo Version { get; set; }
	public Protocol Protocol { get; set; }
}

internal class Program
{
	private static async Task Main(string[] args)
	{



		string dataPath = @"C:\Users\Title\source\repos\Minecraft-Holy-Client\src\McProtoNet\SourceGenerator\minecraft-data-master\minecraft-data-master\data\";

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






	}







	private static async Task GenerateFileNames(Protocol protocol)
	{
		var serverPackets = (protocol.Namespaces["play"] as Namespace).Types["toClient"] as Namespace;
		var clientPackets = (protocol.Namespaces["play"] as Namespace).Types["toServer"] as Namespace;



		var typesNames = GenerateNames(protocol.Types);

		var clientPacketsNames = GenerateNames(clientPackets.Types);

		var serverPacketsNames = GenerateNames(serverPackets.Types);

		Dictionary<string, Dictionary<string, string>> names = new();


		names["Types"] = typesNames;
		names["ClientPackets"] = clientPacketsNames;
		names["ServerPackets"] = serverPacketsNames;

		using (var fs = File.OpenWrite("names.json"))
		{
			await JsonSerializer.SerializeAsync(fs, names, options: new JsonSerializerOptions
			{
				WriteIndented = true
			});
		}



	}

	private static Dictionary<string, string> GenerateNames(Dictionary<string, ProtodefType> types)
	{
		Dictionary<string, string> names = new();
		foreach (var (key, val) in types)
		{
			if (val is IPathTypeEnumerable enumerable)
			{
				FillNames(enumerable, key, names);
			}
		}

		return names;


	}

	private static void FillNames(IPathTypeEnumerable enumerable, string parent, Dictionary<string, string> names)
	{

		foreach (var item in enumerable)
		{
			string path = parent + "." + item.Key;



			if (item.Value is IPathTypeEnumerable en)
			{

				bool any = false;
				foreach (var field in en)
				{
					any = true;
					break;
				}
				if (any)
				{
					FillNames(en, path, names);

					if (item.Value.IsContainer())
					{
						var generatedName = Helpers.ToPascalCase(path.Replace(".", "_").Replace(":", "_"));

						names[path] = generatedName;
					}

				}
			}
			else if (item.Value is ProtodefBitField)
			{
				var generatedName = Helpers.ToPascalCase(path.Replace(".", "_").Replace(":", "_"));
				names[path] = generatedName;
			}
		}
	}


}
