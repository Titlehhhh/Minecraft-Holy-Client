namespace McProtoNet
{
	public enum ClientState
	{
		None,
		Connecting,
		HandShake,
		Login,
		Play
	}
	public struct ClientStateChanged
	{
		public ClientState OldValue { get; }
		public ClientState NewValue { get; }

		public ClientStateChanged(ClientState oldValue, ClientState newValue)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}
	}
}