﻿using System.Collections.Frozen;

namespace McProtoNet
{
	public class PacketPalette_1_16 : IPacketPallete
	{

		public static PacketPalette_1_16 Instance { get; } = new();

		private readonly FrozenDictionary<int, PacketIn> typeIn = FrozenDictionary.ToFrozenDictionary(new Dictionary<int, PacketIn>()
		{
		   { 0x00, PacketIn.SpawnEntity },
			{ 0x01, PacketIn.SpawnExperienceOrb },
			{ 0x02, PacketIn.SpawnLivingEntity },
			{ 0x03, PacketIn.SpawnPainting },
			{ 0x04, PacketIn.SpawnPlayer },
			{ 0x05, PacketIn.EntityAnimation },
			{ 0x06, PacketIn.Statistics },
			{ 0x07, PacketIn.AcknowledgePlayerAction },
			{ 0x08, PacketIn.BlockBreakAnimation },
			{ 0x09, PacketIn.BlockEntityData },
			{ 0x0A, PacketIn.BlockAction },
			{ 0x0B, PacketIn.BlockChange },
			{ 0x0C, PacketIn.BossBar },
			{ 0x0D, PacketIn.ServerDifficulty },
			{ 0x0E, PacketIn.ChatMessage },
			{ 0x0F, PacketIn.MultiBlockChange },
			{ 0x10, PacketIn.TabComplete },
			{ 0x11, PacketIn.DeclareCommands },
			{ 0x12, PacketIn.WindowConfirmation },
			{ 0x13, PacketIn.CloseWindow },
			{ 0x14, PacketIn.WindowItems },
			{ 0x15, PacketIn.WindowProperty },
			{ 0x16, PacketIn.SetSlot },
			{ 0x17, PacketIn.SetCooldown },
			{ 0x18, PacketIn.PluginMessage },
			{ 0x19, PacketIn.NamedSoundEffect },
			{ 0x1A, PacketIn.Disconnect },
			{ 0x1B, PacketIn.EntityStatus },
			{ 0x1C, PacketIn.Explosion },
			{ 0x1D, PacketIn.UnloadChunk },
			{ 0x1E, PacketIn.ChangeGameState },
			{ 0x1F, PacketIn.OpenHorseWindow },
			{ 0x20, PacketIn.KeepAlive },
			{ 0x21, PacketIn.ChunkData },
			{ 0x22, PacketIn.Effect },
			{ 0x23, PacketIn.Particle },
			{ 0x24, PacketIn.UpdateLight },
			{ 0x25, PacketIn.JoinGame },
			{ 0x26, PacketIn.MapData },
			{ 0x27, PacketIn.TradeList },
			{ 0x28, PacketIn.EntityPosition },
			{ 0x29, PacketIn.EntityPositionRotation },
			{ 0x2A, PacketIn.EntityRotation },
			{ 0x2B, PacketIn.EntityMovement },
			{ 0x2C, PacketIn.VehicleMove },
			{ 0x2D, PacketIn.OpenBook },
			{ 0x2E, PacketIn.OpenWindow },
			{ 0x2F, PacketIn.OpenSignEditor },
			{ 0x30, PacketIn.CraftRecipeResponse },
			{ 0x31, PacketIn.PlayerAbilities },
			{ 0x32, PacketIn.CombatEvent },
			{ 0x33, PacketIn.PlayerInfo },
			{ 0x34, PacketIn.FacePlayer },
			{ 0x35, PacketIn.PlayerPositionRotation },
			{ 0x36, PacketIn.UnlockRecipes },
			{ 0x37, PacketIn.DestroyEntities },
			{ 0x38, PacketIn.RemoveEntityEffect },
			{ 0x39, PacketIn.ResourcePackSend },
			{ 0x3A, PacketIn.Respawn },
			{ 0x3B, PacketIn.EntityHeadLook },
			{ 0x3C, PacketIn.SelectAdvancementTab },
			{ 0x3D, PacketIn.WorldBorder },
			{ 0x3E, PacketIn.Camera },
			{ 0x3F, PacketIn.HeldItemChange },
			{ 0x40, PacketIn.UpdateViewPosition },
			{ 0x41, PacketIn.UpdateViewDistance },
			{ 0x42, PacketIn.SpawnPosition },
			{ 0x43, PacketIn.DisplayScoreboard },
			{ 0x44, PacketIn.EntityMetadata },
			{ 0x45, PacketIn.AttachEntity },
			{ 0x46, PacketIn.EntityVelocity },
			{ 0x47, PacketIn.EntityEquipment },
			{ 0x48, PacketIn.SetExperience },
			{ 0x49, PacketIn.UpdateHealth },
			{ 0x4A, PacketIn.ScoreboardObjective },
			{ 0x4B, PacketIn.SetPassengers },
			{ 0x4C, PacketIn.Teams },
			{ 0x4D, PacketIn.UpdateScore },
			{ 0x4E, PacketIn.TimeUpdate },
			{ 0x4F, PacketIn.Title },
			{ 0x50, PacketIn.EntitySoundEffect },
			{ 0x51, PacketIn.SoundEffect },
			{ 0x52, PacketIn.StopSound },
			{ 0x53, PacketIn.PlayerListHeaderAndFooter },
			{ 0x54, PacketIn.NBTQueryResponse },
			{ 0x55, PacketIn.CollectItem },
			{ 0x56, PacketIn.EntityTeleport },
			{ 0x57, PacketIn.Advancements },
			{ 0x58, PacketIn.EntityProperties },
			{ 0x59, PacketIn.EntityEffect },
			{ 0x5A, PacketIn.DeclareRecipes },
			{ 0x5B, PacketIn.Tags },
		});

		private readonly FrozenDictionary<PacketOut, int> typeOut = FrozenDictionary.ToFrozenDictionary(new Dictionary<PacketOut, int>()
		{
		   { PacketOut.TeleportConfirm, 0x00 },
			{ PacketOut.QueryBlockNBT, 0x01 },
			{ PacketOut.SetDifficulty, 0x02 },
			{ PacketOut.ChatMessage, 0x03 },
			{ PacketOut.ClientStatus, 0x04 },
			{ PacketOut.ClientSettings, 0x05 },
			{ PacketOut.TabComplete, 0x06 },
			{ PacketOut.WindowConfirmation, 0x07 },
			{ PacketOut.ClickWindowButton, 0x08 },
			{ PacketOut.ClickWindow, 0x09 },
			{ PacketOut.CloseWindow, 0x0A },
			{ PacketOut.PluginMessage, 0x0B },
			{ PacketOut.EditBook, 0x0C },
			{ PacketOut.EntityNBTRequest, 0x0D },
			{ PacketOut.InteractEntity, 0x0E },
			{ PacketOut.GenerateStructure, 0x0F },
			{ PacketOut.KeepAlive, 0x10 },
			{ PacketOut.LockDifficulty, 0x11 },
			{ PacketOut.PlayerPosition, 0x12 },
			{ PacketOut.PlayerPositionRotation, 0x13 },
			{ PacketOut.PlayerRotation, 0x14 },
			{ PacketOut.PlayerMovement, 0x15 },
			{ PacketOut.VehicleMove, 0x16 },
			{ PacketOut.SteerBoat, 0x17 },
			{ PacketOut.PickItem, 0x18 },
			{ PacketOut.CraftRecipeRequest, 0x19 },
			{ PacketOut.PlayerAbilities, 0x1A },
			{ PacketOut.PlayerAction, 0x1B },
			{ PacketOut.EntityAction, 0x1C },
			{ PacketOut.SteerVehicle, 0x1D },
			{ PacketOut.RecipeBookData, 0x1E },
			{ PacketOut.NameItem, 0x1F },
			{ PacketOut.ResourcePackStatus, 0x20 },
			{ PacketOut.AdvancementTab, 0x21 },
			{ PacketOut.SelectTrade, 0x22 },
			{ PacketOut.SetBeaconEffect, 0x23 },
			{ PacketOut.HeldItemChange, 0x24 },
			{ PacketOut.UpdateCommandBlock, 0x25 },
			{ PacketOut.UpdateCommandBlockMinecart, 0x26 },
			{ PacketOut.CreativeInventoryAction, 0x27 },
			{ PacketOut.UpdateJigsawBlock, 0x28 },
			{ PacketOut.UpdateStructureBlock, 0x29 },
			{ PacketOut.UpdateSign, 0x2A },
			{ PacketOut.Animation, 0x2B },
			{ PacketOut.Spectate, 0x2C },
			{ PacketOut.PlayerBlockPlacement, 0x2D },
			{ PacketOut.UseItem, 0x2E },
		});

		public int GetOut(PacketOut packet)
		{
			return typeOut[packet];
		}

		public bool TryGetIn(int id, out PacketIn packetIn)
		{
			if (typeIn.TryGetValue(id, out packetIn))
			{
				return true;
			}
			return false;
		}
	}
}
