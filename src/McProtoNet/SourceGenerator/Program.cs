using SourceGenerator.ProtoDefTypes;


internal class Program
{
	private static void Main(string[] args)
	{


		using var sr = new StreamReader("protocol.json");


		string json = sr.ReadToEnd();


		var proto = new ProtodefParser(json).Parse();



		

		//Console.ReadLine();
	}
}