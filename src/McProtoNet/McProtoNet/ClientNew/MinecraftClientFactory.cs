namespace McProtoNet.ClientNew
{
	public sealed class MinecraftClientFactory
	{

		public IMinecraftClient Create(MinecraftClientOptions options)
		{
			if (options.ProtocolVersion == 754)
			{

			}
			throw new NotImplementedException();
		}
	}
}
