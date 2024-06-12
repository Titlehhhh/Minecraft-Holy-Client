using McProtoNet.Serialization;
namespace McProtoNet.Protocol755
{
	public sealed class Protocol_755
	{

		public Task SendTeleportConfirm(int teleportId)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x00);
			writer.WriteVarInt(teleportId);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendSetDifficulty(byte newDifficulty)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x02);
			writer.WriteUnsignedByte(newDifficulty);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendQueryEntityNbt(int transactionId, int entityId)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x0c);
			writer.WriteVarInt(transactionId);
			writer.WriteVarInt(entityId);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendPickItem(int slot)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x17);
			writer.WriteVarInt(slot);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendNameItem(string name)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x20);
			writer.WriteString(name);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendSelectTrade(int slot)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x23);
			writer.WriteVarInt(slot);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendSetBeaconEffect(int primary_effect, int secondary_effect)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x24);
			writer.WriteVarInt(primary_effect);
			writer.WriteVarInt(secondary_effect);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendUpdateCommandBlockMinecart(int entityId, string command, bool track_output)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x27);
			writer.WriteVarInt(entityId);
			writer.WriteString(command);
			writer.WriteBoolean(track_output);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendTabComplete(int transactionId, string text)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x06);
			writer.WriteVarInt(transactionId);
			writer.WriteString(text);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendChat(string message)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x03);
			writer.WriteString(message);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendClientCommand(int actionId)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x04);
			writer.WriteVarInt(actionId);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendSettings(string locale, sbyte viewDistance, int chatFlags, bool chatColors, byte skinParts, int mainHand, bool disableTextFiltering)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x05);
			writer.WriteString(locale);
			writer.WriteSignedByte(viewDistance);
			writer.WriteVarInt(chatFlags);
			writer.WriteBoolean(chatColors);
			writer.WriteUnsignedByte(skinParts);
			writer.WriteVarInt(mainHand);
			writer.WriteBoolean(disableTextFiltering);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendEnchantItem(sbyte windowId, sbyte enchantment)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x07);
			writer.WriteSignedByte(windowId);
			writer.WriteSignedByte(enchantment);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendCloseWindow(byte windowId)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x09);
			writer.WriteUnsignedByte(windowId);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendKeepAlive(long keepAliveId)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x0f);
			writer.WriteSignedLong(keepAliveId);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendLockDifficulty(bool locked)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x10);
			writer.WriteBoolean(locked);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendPosition(double x, double y, double z, bool onGround)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x11);
			writer.WriteDouble(x);
			writer.WriteDouble(y);
			writer.WriteDouble(z);
			writer.WriteBoolean(onGround);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendPositionLook(double x, double y, double z, float yaw, float pitch, bool onGround)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x12);
			writer.WriteDouble(x);
			writer.WriteDouble(y);
			writer.WriteDouble(z);
			writer.WriteFloat(yaw);
			writer.WriteFloat(pitch);
			writer.WriteBoolean(onGround);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendLook(float yaw, float pitch, bool onGround)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x13);
			writer.WriteFloat(yaw);
			writer.WriteFloat(pitch);
			writer.WriteBoolean(onGround);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendFlying(bool onGround)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x14);
			writer.WriteBoolean(onGround);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendVehicleMove(double x, double y, double z, float yaw, float pitch)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x15);
			writer.WriteDouble(x);
			writer.WriteDouble(y);
			writer.WriteDouble(z);
			writer.WriteFloat(yaw);
			writer.WriteFloat(pitch);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendSteerBoat(bool leftPaddle, bool rightPaddle)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x16);
			writer.WriteBoolean(leftPaddle);
			writer.WriteBoolean(rightPaddle);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendCraftRecipeRequest(sbyte windowId, string recipe, bool makeAll)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x18);
			writer.WriteSignedByte(windowId);
			writer.WriteString(recipe);
			writer.WriteBoolean(makeAll);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendAbilities(sbyte flags)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x19);
			writer.WriteSignedByte(flags);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendEntityAction(int entityId, int actionId, int jumpBoost)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x1b);
			writer.WriteVarInt(entityId);
			writer.WriteVarInt(actionId);
			writer.WriteVarInt(jumpBoost);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendSteerVehicle(float sideways, float forward, byte jump)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x1c);
			writer.WriteFloat(sideways);
			writer.WriteFloat(forward);
			writer.WriteUnsignedByte(jump);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendDisplayedRecipe(string recipeId)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x1f);
			writer.WriteString(recipeId);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendRecipeBook(int bookId, bool bookOpen, bool filterActive)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x1e);
			writer.WriteVarInt(bookId);
			writer.WriteBoolean(bookOpen);
			writer.WriteBoolean(filterActive);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendResourcePackReceive(int result)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x21);
			writer.WriteVarInt(result);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendHeldItemSlot(short slotId)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x25);
			writer.WriteSignedShort(slotId);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendArmAnimation(int hand)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x2c);
			writer.WriteVarInt(hand);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendSpectate(Guid target)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x2d);
			writer.WriteUUID(target);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendUseItem(int hand)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x2f);
			writer.WriteVarInt(hand);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}
		public Task SendPong(int id)
		{
			var writer = new MinecraftPrimitiveWriterSlim();
			writer.WriteVarInt(0x1d);
			writer.WriteSignedInt(id);
			var buffer_codeGen = writer.GetWrittenMemory();
			try
			{
				return Task.CompletedTask;
			}
			finally
			{
				buffer_codeGen.Dispose();
			}
		}


	}

}