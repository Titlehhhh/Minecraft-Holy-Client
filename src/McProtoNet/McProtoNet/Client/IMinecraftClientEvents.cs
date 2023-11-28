using McProtoNet.Events;

namespace McProtoNet
{
	public interface IMinecraftClientEvents
	{
		ClientConfig Config { get; set; }
		IObservable<ChatMessageEventArgs> OnChatMessage { get; }
		IObservable<DisconnectEventArgs> OnDisconnect { get; }
		IObservable<EntityPositionEventArgs> OnEntityPosition { get; }
		IObservable<EntityPositionRotationEventArgs> OnEntityPositionRotation { get; }
		IObservable<EntityTeleportEventArgs> OnEntityTeleport { get; }
		IObservable<JoinGameEventArgs> OnJoinGame { get; }
		IObservable<MapDataEventArgs> OnMapData { get; }
		IObservable<PlayerPositionRotationEventArgs> OnPlayerPositionRotation { get; }
		IObservable<RespawnEventArgs> OnRespawn { get; }
		IObservable<SpawnPlayerEventArgs> OnSpawnPlayer { get; }
	}
}