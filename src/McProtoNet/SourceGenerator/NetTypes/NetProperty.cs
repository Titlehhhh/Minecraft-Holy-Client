namespace SourceGenerator.NetTypes
{
	public sealed class NetProperty : NetType
	{
		public string Modifier { get; set; } = "public";
		public string Name { get; set; }

		public string Type { get; set; }

		public string GetSet { get; set; } = "{ get; set; }";

		public override string ToString()
		{
			return $"{Modifier} {Type} {Name} {GetSet}";
		}
	}

}
