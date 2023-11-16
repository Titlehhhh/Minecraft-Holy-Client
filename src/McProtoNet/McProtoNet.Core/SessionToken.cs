

namespace McProtoNet
{
	public struct SessionToken
	{
		public string UUID { get; private set; }
		public string SessionId { get; private set; }
		public string Username { get; private set; }
		public SessionToken(string uuid, string sessionid, string user)
		{
			UUID = uuid;
			SessionId = sessionid;
			Username = user;

		}
		public SessionToken(string user)
		{
			UUID = "";
			SessionId = "";
			Username = user;
		}
	}
}
