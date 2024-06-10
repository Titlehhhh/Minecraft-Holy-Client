namespace McProtoNet.Protocol
{

	public struct CommandNodeFlags
	{
		internal CommandNodeFlags(byte value)
		{

		}



		public const int Size = 8;


		public readonly ushort Unused;
		public readonly bool HasCustomSuggestions;
		public readonly bool HasRedirectNode;
		public readonly bool HasCommand;
		public readonly byte CommandNodeType;
	}
}
