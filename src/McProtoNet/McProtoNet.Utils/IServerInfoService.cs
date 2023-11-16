namespace McProtoNet.Utils
{
	public interface IServerInfoService
	{
		Task<ServerInfo> GetServerInfoAsync(string host, ushort port);
	}
}