


namespace McProtoNet
{
	public interface IMinecraftClient : IMinecraftClientActions, IMinecraftClientEvents, IDisposable, IAsyncDisposable
	{
		public ClientState State { get; }
		public event EventHandler<StateChangedEventArgs> StateChanged;
		ClientConfig Config { get; set; }


		void Start();


	}
}