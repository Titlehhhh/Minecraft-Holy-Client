using SourceGenerator.ProtoDefTypes;
using System.Linq;
using System.Xml.Linq;


internal class Program
{
	private static async Task Main(string[] args)
	{

		ProtodefParser parser = new(await File.ReadAllTextAsync("protocol.json"));

		var protocol = parser.Parse();

		var packets = (protocol.Namespaces["play"] as Namespace).Types["toClient"] as Namespace;


		List<string> names = new();

		foreach (var (key, val) in packets.Types)
		{
			if (val is IPathTypeEnumerable enumerable)
			{
				EnumerateTree(enumerable, key, names);
			}
		}



		foreach (var name in names)
		{
			Console.WriteLine(name);
		}

	}

	private static void EnumerateTree(IPathTypeEnumerable enumerable, string parent, List<string> names)
	{
		int id = 0;
		foreach (var item in enumerable)
		{
			id++;
			string name = string.IsNullOrEmpty(item.Key) ? $"anon_{id}" : item.Key;
			name = parent + "." + name;



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
					EnumerateTree(en, name, names);

					if (!item.Value.IsSwitch() || !item.Value.IsArray())
						names.Add(name);

				}
			}
		}
	}


}
