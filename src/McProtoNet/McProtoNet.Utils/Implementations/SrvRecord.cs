namespace McProtoNet.Utils
{
	public class SrvRecord
	{
		private string target;
		private ushort port;

		public SrvRecord(string target, ushort port)
		{
			this.target = target;
			this.port = port;
		}

		public string Target { get => target;  }
		public ushort Port { get => port;}
	}
}