using McProtoNet.Core;
using McProtoNet.Core.IO;
using McProtoNet.Core.Protocol;

namespace McProtoNet
{
	public partial class MinecraftClient : IMinecraftClientEvents, IMinecraftClientActions
	{
		private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
		public ValueTask SendPacket(Action<IMinecraftPrimitiveWriter> action, PacketOut id)
		{

			return SendPacket(action, _packetPallete.GetOut(id));

		}

		public async ValueTask SendPacket(Action<IMinecraftPrimitiveWriter> action, int id)
		{

			try
			{
				await semaphore.WaitAsync(CTS.Token);
				using (MemoryStream ms = StaticResources.MSmanager.GetStream())
				{
					var writer = Performance.Writers.Get();
					try
					{


						writer.BaseStream = ms;
						action(writer);
						ms.Position = 0;
					}
					finally
					{
						Performance.Writers.Return(writer);
					}
					await PacketSender.SendPacketAsync(new(id, ms), CTS.Token);

				}
			}
			catch (Exception ex)
			{
				CancelAll(ex);
				InvokeError();
				throw;
			}
			finally
			{
				semaphore.Release();
			}




		}
		public async ValueTask SendPacketAsync(IOutputPacket packet, int id)
		{

			try
			{
				await semaphore.WaitAsync(CTS.Token);
				using (MemoryStream ms = StaticResources.MSmanager.GetStream())
				{
					var writer = Performance.Writers.Get();
					try
					{

						writer.BaseStream = ms;

						packet.Write(writer);

					}
					finally
					{
						Performance.Writers.Return(writer);
					}
					ms.Position = 0;
					await PacketSender.SendPacketAsync(new(id, ms), CTS.Token);
				}
			}
			catch (Exception ex)
			{
				CancelAll(ex);
				throw;
			}
			finally
			{
				semaphore.Release();
			}

		}

		public ValueTask SendChat(string text)
		{
			return SendPacket(w =>
			{
				w.WriteString(text);
			}, PacketOut.ChatMessage);
		}


		public ValueTask SendAction(int type, Vector3 position, BlockFace face)
		{
			return SendPacket(w =>
			{
				w.WriteVarInt(type);
				w.Write(GetLocation(position));
				w.WriteVarInt(face);
				if (_protocol >= MinecraftVersion.MC_1_19_Version)
					w.WriteVarInt(0);
			}, PacketOut.PlayerAction);
		}
		public ValueTask SendPositionRotation(double x, double y, double z, float yaw, float pitch, bool onGround)
		{
			return this.SendPacket(w =>
			{
				w.WriteDouble(x);
				w.WriteDouble(y);
				w.WriteDouble(z);
				w.WriteFloat(yaw);
				w.WriteFloat(pitch);
				w.WriteBoolean(onGround);
			}, PacketOut.PlayerPositionRotation);

		}
		public ValueTask SendPosition(double x, double y, double z, bool onGround)
		{
			return this.SendPacket(w =>
			{
				w.WriteDouble(x);
				w.WriteDouble(y);
				w.WriteDouble(z);

				w.WriteBoolean(onGround);
			}, PacketOut.PlayerPosition);

		}
		public ValueTask SendRotation(float yaw, float pitch, bool onGround)
		{
			return this.SendPacket(w =>
			{
				w.WriteFloat(yaw);
				w.WriteFloat(pitch);
				w.WriteBoolean(onGround);
			}, PacketOut.PlayerRotation);

		}

		public ValueTask SendTeleportConfirm(int id)
		{
			return this.SendPacket(w =>
			{
				w.WriteVarInt(id);
			}, PacketOut.TeleportConfirm);
		}


		public ValueTask SendPluginMessage(string channel, byte[] data)
		{
			return this.SendPacket(w =>
			{
				w.WriteString(channel);
				w.Write(data);
			}, PacketOut.PluginMessage);
		}

		public ValueTask SendSettings(string language, byte viewDistance, byte chatMode, bool chatColors, byte skinParts, byte mainHand)
		{
			return this.SendPacket(w =>
			{
				w.WriteString(language);
				w.WriteUnsignedByte(viewDistance);
				w.WriteVarInt(chatMode);
				w.WriteBoolean(chatColors);
				w.WriteUnsignedByte(skinParts);
				w.WriteVarInt(mainHand);

				if (_protocol >= MinecraftVersion.MC_1_17_Version)
				{
					if (_protocol >= MinecraftVersion.MC_1_18_1_Version)
						w.WriteUnsignedByte(0); // 1.18 and above - Enable text filtering. (Always false)
					else
						w.WriteUnsignedByte(1); // 1.17 and 1.17.1 - Disable text filtering. (Always true)
				}
				if (_protocol >= MinecraftVersion.MC_1_18_1_Version)
					w.WriteUnsignedByte(1); // 1.18 and above - Allow server listings

			}, PacketOut.ClientSettings);
		}

		public ValueTask SendUseItem(int hand)
		{
			return this.SendPacket(w =>
			{
				w.WriteVarInt(hand);

			}, PacketOut.UseItem);
		}




		private byte[] GetLocation(Vector3 location)
		{
			byte[] locationBytes;
			if (_protocol >= MinecraftVersion.MC_1_14_Version)
			{
				locationBytes = BitConverter.GetBytes(((((ulong)location.X) & 0x3FFFFFF) << 38) | ((((ulong)location.Z) & 0x3FFFFFF) << 12) | (((ulong)location.Y) & 0xFFF));
			}
			else locationBytes = BitConverter.GetBytes(((((ulong)location.X) & 0x3FFFFFF) << 38) | ((((ulong)location.Y) & 0xFFF) << 26) | (((ulong)location.Z) & 0x3FFFFFF));
			Array.Reverse(locationBytes); //Endianness
			return locationBytes;
		}
	}
}
