namespace McProtoNet.ClientNew
{
	public interface IMinecraftLogin
	{
		public Task<LoginizationResult> Login(Stream source, LoginOptions options, CancellationToken cancellationToken = default);
	}
}
