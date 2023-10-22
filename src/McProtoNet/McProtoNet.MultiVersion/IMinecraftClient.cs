using McProtoNet.Core;
using McProtoNet.Core.IO;
using McProtoNet.Geometry;
using McProtoNet.MultiVersion.Client;
using McProtoNet.MultiVersion.Events;


namespace McProtoNet.MultiVersion
{
	public interface IMinecraftClient : IMinecraftClientActions, IMinecraftClientEvents, IDisposable, IAsyncDisposable
	{
		public ClientState State { get; }
		public event EventHandler<StateChangedEventArgs> StateChanged;


		Task Disconnect();
	}
}