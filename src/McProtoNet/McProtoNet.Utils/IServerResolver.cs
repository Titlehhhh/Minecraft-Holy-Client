namespace McProtoNet.Utils
{

	public interface IServerResolver
	{
		Task<SrvRecord> ResolveAsync(string host, CancellationToken cancellationToken = default);
	}
}