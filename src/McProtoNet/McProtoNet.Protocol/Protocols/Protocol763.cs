using McProtoNet.Serialization;using McProtoNet.Protocol;

namespace McProtoNet.Protocol763
{
	public sealed class Protocol_763 : ProtocolBase
{
	
	
		public Task SendTeleportConfirm(int teleportId)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x00);
			writer.WriteVarInt(teleportId);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendChatMessage(string message, long timestamp, long salt, byte[]? signature, int offset, byte[] acknowledged)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x05);
			writer.WriteString(message);
			writer.WriteSignedLong(timestamp);
			writer.WriteSignedLong(salt);
			if (signature is null)
			{
				writer.WriteBoolean(false);
			}
			else
			{
				writer.WriteBoolean(true);
				writer.WriteVarInt(256);
			writer.WriteBuffer(signature);
			}
			
			writer.WriteVarInt(offset);
			writer.WriteVarInt(3);
			writer.WriteBuffer(acknowledged);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendSetDifficulty(byte newDifficulty)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x02);
			writer.WriteUnsignedByte(newDifficulty);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendMessageAcknowledgement(int count)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x03);
			writer.WriteVarInt(count);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendEditBook(int hand, string[] pages, string? title)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x0e);
			writer.WriteVarInt(hand);
			writer.WriteVarInt(pages.Length);
			for (int i_0 = 0; i_0 < pages.Length; i_0++)
			{
				var value_0 = pages[i_0];
				writer.WriteString(value_0);
			}
			if (title is null)
			{
				writer.WriteBoolean(false);
			}
			else
			{
				writer.WriteBoolean(true);
				writer.WriteString(title);
			}
			
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendQueryEntityNbt(int transactionId, int entityId)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x0f);
			writer.WriteVarInt(transactionId);
			writer.WriteVarInt(entityId);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendPickItem(int slot)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x1a);
			writer.WriteVarInt(slot);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendNameItem(string name)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x23);
			writer.WriteString(name);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendSelectTrade(int slot)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x26);
			writer.WriteVarInt(slot);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendSetBeaconEffect(int? primary_effect, int? secondary_effect)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x27);
			if (primary_effect is null)
			{
				writer.WriteBoolean(false);
			}
			else
			{
				writer.WriteBoolean(true);
				writer.WriteVarInt(primary_effect);
			}
			
			if (secondary_effect is null)
			{
				writer.WriteBoolean(false);
			}
			else
			{
				writer.WriteBoolean(true);
				writer.WriteVarInt(secondary_effect);
			}
			
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendUpdateCommandBlockMinecart(int entityId, string command, bool track_output)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x2a);
			writer.WriteVarInt(entityId);
			writer.WriteString(command);
			writer.WriteBoolean(track_output);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendTabComplete(int transactionId, string text)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x09);
			writer.WriteVarInt(transactionId);
			writer.WriteString(text);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendClientCommand(int actionId)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x07);
			writer.WriteVarInt(actionId);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendSettings(string locale, sbyte viewDistance, int chatFlags, bool chatColors, byte skinParts, int mainHand, bool enableTextFiltering, bool enableServerListing)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x08);
			writer.WriteString(locale);
			writer.WriteSignedByte(viewDistance);
			writer.WriteVarInt(chatFlags);
			writer.WriteBoolean(chatColors);
			writer.WriteUnsignedByte(skinParts);
			writer.WriteVarInt(mainHand);
			writer.WriteBoolean(enableTextFiltering);
			writer.WriteBoolean(enableServerListing);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendEnchantItem(sbyte windowId, sbyte enchantment)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x0a);
			writer.WriteSignedByte(windowId);
			writer.WriteSignedByte(enchantment);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendCloseWindow(byte windowId)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x0c);
			writer.WriteUnsignedByte(windowId);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendKeepAlive(long keepAliveId)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x12);
			writer.WriteSignedLong(keepAliveId);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendLockDifficulty(bool locked)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x13);
			writer.WriteBoolean(locked);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendPosition(double x, double y, double z, bool onGround)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x14);
			writer.WriteDouble(x);
			writer.WriteDouble(y);
			writer.WriteDouble(z);
			writer.WriteBoolean(onGround);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendPositionLook(double x, double y, double z, float yaw, float pitch, bool onGround)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x15);
			writer.WriteDouble(x);
			writer.WriteDouble(y);
			writer.WriteDouble(z);
			writer.WriteFloat(yaw);
			writer.WriteFloat(pitch);
			writer.WriteBoolean(onGround);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendLook(float yaw, float pitch, bool onGround)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x16);
			writer.WriteFloat(yaw);
			writer.WriteFloat(pitch);
			writer.WriteBoolean(onGround);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendFlying(bool onGround)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x17);
			writer.WriteBoolean(onGround);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendVehicleMove(double x, double y, double z, float yaw, float pitch)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x18);
			writer.WriteDouble(x);
			writer.WriteDouble(y);
			writer.WriteDouble(z);
			writer.WriteFloat(yaw);
			writer.WriteFloat(pitch);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendSteerBoat(bool leftPaddle, bool rightPaddle)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x19);
			writer.WriteBoolean(leftPaddle);
			writer.WriteBoolean(rightPaddle);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendCraftRecipeRequest(sbyte windowId, string recipe, bool makeAll)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x1b);
			writer.WriteSignedByte(windowId);
			writer.WriteString(recipe);
			writer.WriteBoolean(makeAll);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendAbilities(sbyte flags)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x1c);
			writer.WriteSignedByte(flags);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendEntityAction(int entityId, int actionId, int jumpBoost)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x1e);
			writer.WriteVarInt(entityId);
			writer.WriteVarInt(actionId);
			writer.WriteVarInt(jumpBoost);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendSteerVehicle(float sideways, float forward, byte jump)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x1f);
			writer.WriteFloat(sideways);
			writer.WriteFloat(forward);
			writer.WriteUnsignedByte(jump);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendDisplayedRecipe(string recipeId)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x22);
			writer.WriteString(recipeId);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendRecipeBook(int bookId, bool bookOpen, bool filterActive)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x21);
			writer.WriteVarInt(bookId);
			writer.WriteBoolean(bookOpen);
			writer.WriteBoolean(filterActive);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendResourcePackReceive(int result)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x24);
			writer.WriteVarInt(result);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendHeldItemSlot(short slotId)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x28);
			writer.WriteSignedShort(slotId);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendArmAnimation(int hand)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x2f);
			writer.WriteVarInt(hand);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendSpectate(Guid target)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x30);
			writer.WriteUUID(target);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendUseItem(int hand, int sequence)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x32);
			writer.WriteVarInt(hand);
			writer.WriteVarInt(sequence);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendPong(int id)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x20);
			writer.WriteSignedInt(id);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
		public Task SendChatSessionUpdate(Guid sessionUUID, long expireTime, byte[] publicKey, byte[] signature)
		{
			scoped var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x06);
			writer.WriteUUID(sessionUUID);
			writer.WriteSignedLong(expireTime);
			writer.WriteVarInt(publicKey.Length);
			writer.WriteBuffer(publicKey);
			writer.WriteVarInt(signature.Length);
			writer.WriteBuffer(signature);
			return base.SendPacketCore(writer.GetWrittenMemory());
		}
	
	
	}

}