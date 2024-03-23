using SourceGenerator.ProtoDefTypes;


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
			if(val is IFieldsEnumerable enumerable)
			{
				foreach(var item in enumerable)
				{

				}
			}
		}


		foreach (var name in names)
		{
			Console.WriteLine(name);
		}

	}


}
