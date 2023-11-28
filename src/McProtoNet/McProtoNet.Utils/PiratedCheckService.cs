namespace McProtoNet.Utils
{
	public class PiratedCheckService : ISessionCheckService
	{
		public async Task<bool> Check(string uuid, string accesToken, string serverHash)
		{
			return true;
		}
	}
}