using SourceGenerator.ProtoDefTypes;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;


internal class Program
{
	private static async Task Main(string[] args)
	{

		ProtodefParser parser = new(await File.ReadAllTextAsync("protocol.json"));

		var protocol = parser.Parse();


		await GenerateFileNames(protocol);



		var nameResolver =
			JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(
				await File.ReadAllTextAsync("names.json"));







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
