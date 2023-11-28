using McProtoNet.Core.IO;
using McProtoNet.Events;
using Microsoft.Extensions.ObjectPool;
using System.Collections.Concurrent;

namespace McProtoNet
{
	internal static class Performance
	{

		internal static ObjectPool<MinecraftPrimitiveReader> Readers { get; private set; }
		internal static ObjectPool<MinecraftPrimitiveWriter> Writers { get; private set; }

		static Performance()
		{
			DefaultObjectPoolProvider defaultObjectPoolProvider = new();
			ObjectPool<MinecraftPrimitiveReader> _readers = defaultObjectPoolProvider.Create<MinecraftPrimitiveReader>();

			Readers = _readers;


			Writers = defaultObjectPoolProvider.Create<MinecraftPrimitiveWriter>();



		}
	}
	public static class PacketPool
	{
		private readonly static DefaultObjectPoolProvider _poolProvider = new();

		public static readonly ObjectPool<ChatMessageEventArgs> ChatPacketPool = 
			_poolProvider.Create<ChatMessageEventArgs>();

		public static readonly ObjectPool<DisconnectEventArgs> DisconnectEventPool =
			_poolProvider.Create<DisconnectEventArgs>();

		public static readonly ObjectPool<EntityPositionEventArgs> EntityPositionPacketPool =
			_poolProvider.Create<EntityPositionEventArgs>();

		public static readonly ObjectPool<EntityPositionRotationEventArgs> EntityPositionRotationPacketPool =
			_poolProvider.Create<EntityPositionRotationEventArgs>();

		public static readonly ObjectPool<EntityTeleportEventArgs> EntityTeleportPacketPool =
			_poolProvider.Create<EntityTeleportEventArgs>();

		public static readonly ObjectPool<JoinGameEventArgs> JoinGamePacketPool =
			_poolProvider.Create<JoinGameEventArgs>();

		public static readonly ObjectPool<MapDataEventArgs> MapDataPacketPool =
			_poolProvider.Create<MapDataEventArgs>();

		public static readonly ObjectPool<PlayerPositionRotationEventArgs> PlayerPositionRotationPacketPool =
			_poolProvider.Create<PlayerPositionRotationEventArgs>();

		public static readonly ObjectPool<RespawnEventArgs> RespawnPacketPool =
			_poolProvider.Create<RespawnEventArgs>();

		public static readonly ObjectPool<SpawnEntityEventArgs> SpawnEntityPacketPool =
			_poolProvider.Create<SpawnEntityEventArgs>();

		public static readonly ObjectPool<SpawnPlayerEventArgs> SpawnPlayerPacketPool =
			_poolProvider.Create<SpawnPlayerEventArgs>();


		static PacketPool()
		{

		}
	}

}