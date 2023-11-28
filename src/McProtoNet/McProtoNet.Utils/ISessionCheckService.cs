namespace McProtoNet.Utils
{
	public interface ISessionCheckService
	{
		Task<bool> Check(string uuid, string accesToken, string serverHash);
	}
}