using McProtoNet.Core.IO;
using McProtoNet.Events;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;

namespace McProtoNet
{
	public partial class MinecraftClient
	{
		private CompositeDisposable _events = new CompositeDisposable();

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private Subject<T> CreateEvent<T>()
		{
			Subject<T> subject = new Subject<T>();
			_events.Add(subject);

			return subject;
		}

		private void CreateEvents()
		{
			_chatEvent = CreateEvent<ChatMessageEventArgs>();
			_disconnectEvent = CreateEvent<DisconnectEventArgs>();
			_joinGameEvent = CreateEvent<JoinGameEventArgs>();
			_mapDataEvent = CreateEvent<MapDataEventArgs>();
			_playerPositionRotationEvent = CreateEvent<PlayerPositionRotationEventArgs>();
			_respawnEvent = CreateEvent<RespawnEventArgs>();
			_entityPositionEvent = CreateEvent<EntityPositionEventArgs>();
			_entityPositionRotationEvent = CreateEvent<EntityPositionRotationEventArgs>();
			_entityTeleportEvent = CreateEvent<EntityTeleportEventArgs>();
			_spawnPlayerEvent = CreateEvent<SpawnPlayerEventArgs>();
			_openWindowEvent = CreateEvent<WindowOpenEventArgs>();
		}

		private Subject<RespawnEventArgs> _respawnEvent;
		private Subject<ChatMessageEventArgs> _chatEvent;
		private Subject<DisconnectEventArgs> _disconnectEvent;
		private Subject<JoinGameEventArgs> _joinGameEvent;
		private Subject<MapDataEventArgs> _mapDataEvent;
		private Subject<PlayerPositionRotationEventArgs> _playerPositionRotationEvent;
		private Subject<EntityPositionEventArgs> _entityPositionEvent;
		private Subject<EntityPositionRotationEventArgs> _entityPositionRotationEvent;
		private Subject<EntityTeleportEventArgs> _entityTeleportEvent;
		private Subject<SpawnPlayerEventArgs> _spawnPlayerEvent;
		private Subject<WindowOpenEventArgs> _openWindowEvent;




		public IObservable<RespawnEventArgs> OnRespawn => _respawnEvent;
		public IObservable<ChatMessageEventArgs> OnChatMessage => _chatEvent;
		public IObservable<DisconnectEventArgs> OnDisconnect => _disconnectEvent;
		public IObservable<JoinGameEventArgs> OnJoinGame => _joinGameEvent;
		public IObservable<MapDataEventArgs> OnMapData => _mapDataEvent;
		public IObservable<PlayerPositionRotationEventArgs> OnPlayerPositionRotation => _playerPositionRotationEvent;
		public IObservable<EntityPositionEventArgs> OnEntityPosition => _entityPositionEvent;
		public IObservable<EntityPositionRotationEventArgs> OnEntityPositionRotation => _entityPositionRotationEvent;
		public IObservable<EntityTeleportEventArgs> OnEntityTeleport => _entityTeleportEvent;
		public IObservable<SpawnPlayerEventArgs> OnSpawnPlayer => _spawnPlayerEvent;
		public IObservable<WindowOpenEventArgs> OnOpenWindow => _openWindowEvent;

		private void OnPacket(IMinecraftPrimitiveReader reader, PacketIn id)
		{


			if (id == PacketIn.Disconnect)
			{

				string reason = reader.ReadString();

				if (_disconnectEvent.HasObservers)
				{
					var dis = PacketPool.DisconnectEventPool.Get();
					try
					{
						dis.Reason = reason;

						_disconnectEvent.OnNext(dis);
					}
					finally
					{
						PacketPool.DisconnectEventPool.Return(dis);
					}
				}
				throw new DisconnectException(reason);
			}
			if (_joinGameEvent.HasObservers && id == PacketIn.JoinGame)
			{
				var join = PacketPool.JoinGamePacketPool.Get();

				try
				{
					_joinGameEvent.OnNext(join);
				}
				finally
				{
					PacketPool.JoinGamePacketPool.Return(join);
				}
			}
			else if (id == PacketIn.OpenWindow)
			{
				var win_id = reader.ReadVarInt();

				_openWindowEvent.OnNext(new WindowOpenEventArgs(win_id));
			}
			else if (id == PacketIn.MapData)
			{
				return;
				int mapid = reader.ReadVarInt();
				byte scale = reader.ReadUnsignedByte();
				// 1.9 +
				bool trackingPosition = true;
				// 1.14+
				bool locked = false;
				// 1.17+ (locked and trackingPosition switched places)

				int iconCount = 0;

				if (_protocol >= MinecraftVersion.MC_1_17_Version)
				{
					if (_protocol >= MinecraftVersion.MC_1_14_Version)
						locked = reader.ReadBoolean();
					trackingPosition = reader.ReadBoolean();
					if (trackingPosition)
					{
						iconCount = reader.ReadVarInt();
					}
				}
				else
				{
					trackingPosition = reader.ReadBoolean();
					if (_protocol >= MinecraftVersion.MC_1_14_Version)
						locked = reader.ReadBoolean();
					iconCount = reader.ReadVarInt();
				}

				MapIcon[] icons = new MapIcon[iconCount];
				for (int i = 0; i < icons.Length; i++)
				{
					int iconType = 0;
					byte icondirection = 0;
					byte iconX = 0;
					byte iconY = 0;
					string? displayName = null;

					// 1.8 - 1.13
					if (_protocol < MinecraftVersion.MC_1_13_2_Version)
					{
						byte directionAndtype = reader.ReadUnsignedByte();
						byte direction, type;

						// 1.12.2+
						if (_protocol >= MinecraftVersion.MC_1_12_2_Version)
						{
							direction = (byte)(directionAndtype & 0xF);
							type = (byte)((directionAndtype >> 4) & 0xF);
						}
						else // 1.8 - 1.12
						{
							direction = (byte)((directionAndtype >> 4) & 0xF);
							type = (byte)(directionAndtype & 0xF);
						}

						iconType = type;
						icondirection = direction;
					}

					// 1.13.2+
					if (_protocol >= MinecraftVersion.MC_1_13_2_Version)
						iconType = reader.ReadVarInt();

					iconX = reader.ReadUnsignedByte();
					iconY = reader.ReadUnsignedByte();

					// 1.13.2+
					if (_protocol >= MinecraftVersion.MC_1_13_2_Version)
					{
						icondirection = reader.ReadUnsignedByte();

						if (reader.ReadBoolean()) // Has Display Name?
							displayName = reader.ReadString();
					}
					MapIcon mapIcon = new(iconType, iconX, iconY, icondirection, displayName);
					icons[i] = mapIcon;

				}

				MapData? data = null;

				byte columnsUpdated = reader.ReadUnsignedByte(); // width
				byte rowsUpdated = 0; // height
				byte mapCoulmnX = 0;
				byte mapRowZ = 0;
				byte[]? colors = null;

				if (columnsUpdated > 0)
				{
					rowsUpdated = reader.ReadUnsignedByte(); // height
					mapCoulmnX = reader.ReadUnsignedByte();
					mapRowZ = reader.ReadUnsignedByte();
					colors = reader.ReadByteArray();
					//data = new MapData(columnsUpdated, rowsUpdated, mapCoulmnX, mapRowZ, colors);
				}

				//var eventArgs = new MapDataEventArgs(mapid, scale, trackingPosition, locked, icons, data);

				//_mapDataEvent.OnNext(eventArgs);
			}
			else if (id == PacketIn.KeepAlive)
			{
				long pingId = reader.ReadLong();
				SendPacket(w => w.WriteLong(pingId), PacketOut.KeepAlive);
			}
			else if (_playerPositionRotationEvent.HasObservers && id == PacketIn.PlayerPositionRotation)
			{
				var x = reader.ReadDouble();
				var y = reader.ReadDouble();
				var z = reader.ReadDouble();

				var yaw = reader.ReadFloat();
				var pitch = reader.ReadFloat();
				var flags = reader.ReadUnsignedByte();
				var teleportId = reader.ReadVarInt();

				//var events = new PlayerPositionRotationEventArgs(x, y, z, yaw, pitch, flags, teleportId);


				var packet = PacketPool.PlayerPositionRotationPacketPool.Get();
				try
				{


					packet.X = x;
					packet.Y = y;
					packet.Z = z;
					packet.Yaw = yaw;
					packet.Pitch = pitch;
					packet.Flags = flags;
					packet.TeleportId = teleportId;

					_playerPositionRotationEvent.OnNext(packet);
				}
				finally
				{
					PacketPool.PlayerPositionRotationPacketPool.Return(packet);
				}


			}
			else if (_respawnEvent.HasObservers && id == PacketIn.Respawn)
			{
				var respawn = PacketPool.RespawnPacketPool.Get();

				try
				{


					_respawnEvent.OnNext(respawn);
				}
				finally
				{
					PacketPool.RespawnPacketPool.Return(respawn);
				}
			}
			else if (_chatEvent.HasObservers && id == PacketIn.ChatMessage)
			{
				string message = reader.ReadString();

				var chatPacket = PacketPool.ChatPacketPool.Get();

				try
				{
					chatPacket.Message = message;
					_chatEvent.OnNext(chatPacket);
				}
				finally
				{
					PacketPool.ChatPacketPool.Return(chatPacket);
				}
			}
			else if (id == PacketIn.SpawnEntity)
			{
				int entityID = reader.ReadVarInt();
				Guid entityUUID = reader.ReadUUID();

				int entityType = reader.ReadVarInt();



				Double entityX, entityY, entityZ;


				entityX = reader.ReadDouble(); // X
				entityY = reader.ReadDouble(); // Y
				entityZ = reader.ReadDouble(); // Z



				int metadata = -1;
				bool hasData = false;
				byte entityPitch, entityYaw;


				entityPitch = reader.ReadUnsignedByte(); // Pitch
				entityYaw = reader.ReadUnsignedByte(); // Yaw


				entityYaw = reader.ReadUnsignedByte(); // Head Yaw

				reader.ReadVarInt();


				reader.ReadShort();
				reader.ReadShort();
				reader.ReadShort();

				//var spawnEntity = PacketPool.SpawnEntityPacketPool.Get();
				try
				{
					//spawnEntity.Id = entityID;
					//spawnEntity.UUID = entityUUID;
				}
				catch
				{

				}

			}
			else if (id == PacketIn.SpawnPlayer)
			{
				//int EntityID = reader.ReadVarInt();
				//Guid UUID = reader.ReadUUID();

				//double x, y, z;

				//x = reader.ReadDouble();
				//y = reader.ReadDouble();
				//z = reader.ReadDouble();


				//byte Yaw = reader.ReadUnsignedByte();
				//byte Pitch = reader.ReadUnsignedByte();

				//Vector3 playerPosition = new(x, y, z);


			}
			else if (id == PacketIn.EntityPosition)
			{

				//int EntityID = reader.ReadVarInt();

				//Double DeltaX, DeltaY, DeltaZ;

				//DeltaX = Convert.ToDouble(reader.ReadShort());
				//DeltaY = Convert.ToDouble(reader.ReadShort());
				//DeltaZ = Convert.ToDouble(reader.ReadShort());


				//bool OnGround = reader.ReadBoolean();
				//DeltaX = DeltaX / (128 * 32);
				//DeltaY = DeltaY / (128 * 32);
				//DeltaZ = DeltaZ / (128 * 32);

				//var args = new EntityPositionEventArgs(EntityID, DeltaX, DeltaY, DeltaZ, OnGround);

			}
			else if (id == PacketIn.EntityRotation)
			{

			}
			else if (id == PacketIn.EntityPositionRotation)
			{
				//int EntityID = reader.ReadVarInt();

				//Double DeltaX, DeltaY, DeltaZ;


				//DeltaX = Convert.ToDouble(reader.ReadShort());
				//DeltaY = Convert.ToDouble(reader.ReadShort());
				//DeltaZ = Convert.ToDouble(reader.ReadShort());



				//byte _yaw = reader.ReadUnsignedByte();
				//byte _pitch = reader.ReadUnsignedByte();
				//bool OnGround = reader.ReadBoolean();
				//DeltaX = DeltaX / (128 * 32);
				//DeltaY = DeltaY / (128 * 32);
				//DeltaZ = DeltaZ / (128 * 32);

				//var args = new EntityPositionRotationEventArgs(EntityID, DeltaX, DeltaY, DeltaZ, _yaw, _pitch, OnGround);


			}
			else if (id == PacketIn.EntityTeleport)
			{
				int EntityID = reader.ReadVarInt();

				double x, y, z;


				x = reader.ReadDouble();
				y = reader.ReadDouble();
				z = reader.ReadDouble();


				byte EntityYaw = reader.ReadUnsignedByte();
				byte EntityPitch = reader.ReadUnsignedByte();
				bool OnGround = reader.ReadBoolean();

				//var args = new EntityTeleportEventArgs(EntityID, x, y, z, EntityYaw, EntityPitch, OnGround);

			}


		}


	}
}
