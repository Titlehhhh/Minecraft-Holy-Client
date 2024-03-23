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

		foreach (var (key, val) in protocol.Types)
		{
			if (val is IFieldsEnumerable enumerable)
			{
				EnumerateTree(enumerable, key, names);
			}
		}



		foreach (var name in names)
		{
			Console.WriteLine(name);
		}

	}

	private static void EnumerateTree(IFieldsEnumerable enumerable, string parent, List<string> names)
	{
		int id = 0;
		foreach (var item in enumerable)
		{
			id++;
			string name = string.IsNullOrEmpty(item.Key) ? $"anon_{id}" : item.Key;
			name = parent + "." + name;



			if (item.Value is IFieldsEnumerable en)
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

					if (!item.Value.IsSwitch())
						names.Add(name);

				}
			}
		}
	}


}
