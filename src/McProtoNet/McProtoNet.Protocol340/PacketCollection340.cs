using McProtoNet.Core.Packets;
using McProtoNet.Protocol340.Packets.Client;
using McProtoNet.Protocol340.Packets.Client.Game;
using McProtoNet.Protocol340.Packets.Server;


namespace McProtoNet.Protocol340
{
    public class PacketFactory340 : PacketFactory
    {

        protected override (Dictionary<int, Type>, Dictionary<int, Type>) CreatePackets(PacketCategory category)
        {
            Dictionary<int, Type> outPackets = null;
            Dictionary<int, Type> inPackets = null;
            if (category == PacketCategory.Login)
            {
                outPackets = new Dictionary<int, Type>()
                {
                    {0x00, typeof(LoginStartPacket) },
                    {0x01, typeof(EncryptionResponsePacket) },

                };
                inPackets = new Dictionary<int, Type>()
                {
                    {0x00, typeof(LoginDisconnectPacket) },
                    {0x01,typeof(EncryptionRequestPacket) },
                    {0x02,typeof(LoginSuccessPacket) },
                    {0x03,typeof(LoginSetCompressionPacket) }
                };
            }
            else if (category == PacketCategory.Game)
            {
                outPackets = new Dictionary<int, Type>()
                {
                    { 0x00, typeof(ClientTeleportConfirmPacket) },
                    { 0x01, typeof(ClientTabCompletePacket) },
                    { 0x02, typeof(ClientChatPacket) },
                    { 0x03, typeof(ClientRequestPacket) },
                    { 0x04, typeof(ClientSettingsPacket) },
                    { 0x05, typeof(ClientConfirmTransactionPacket) },
                    { 0x06, typeof(ClientEnchantItemPacket) },
                    { 0x07, typeof(ClientWindowActionPacket) },
                    { 0x08, typeof(ClientCloseWindowPacket) },
                    { 0x09, typeof(ClientPluginMessagePacket) },
                    { 0x0A, typeof(ClientPlayerInteractEntityPacket) },
                    { 0x0B, typeof(ClientKeepAlivePacket) },
                    { 0x0C, typeof(ClientPlayerMovementPacket) },
                    { 0x0D, typeof(ClientPlayerPositionPacket) },
                    { 0x0E, typeof(ClientPlayerPositionRotationPacket) },
                    { 0x0F, typeof(ClientPlayerRotationPacket) },
                    { 0x10, typeof(ClientVehicleMovePacket) },
                    { 0x11, typeof(ClientSteerBoatPacket) },
                    { 0x12, typeof(ClientPrepareCraftingGridPacket) },
                    { 0x13, typeof(ClientPlayerAbilitiesPacket) },
                    { 0x14, typeof(ClientPlayerActionPacket) },
                    { 0x15, typeof(ClientPlayerStatePacket) },
                    { 0x16, typeof(ClientSteerVehiclePacket) },
                    { 0x17, typeof(ClientCraftingBookDataPacket) },
                    { 0x18, typeof(ClientResourcePackStatusPacket) },
                    { 0x19, typeof(ClientAdvancementTabPacket) },
                    { 0x1A, typeof(ClientPlayerChangeHeldItemPacket) },
                    { 0x1B, typeof(ClientCreativeInventoryActionPacket) },
                    { 0x1C, typeof(ClientUpdateSignPacket) },
                    { 0x1D, typeof(ClientPlayerSwingArmPacket) },
                    { 0x1E, typeof(ClientSpectatePacket) },
                    { 0x1F, typeof(ClientPlayerPlaceBlockPacket) },
                    { 0x20, typeof(ClientPlayerUseItemPacket) },

                };
                inPackets = new Dictionary<int, Type>()
                {
                    { 0x00, typeof(ServerSpawnObjectPacket) },
                    { 0x01, typeof(ServerSpawnExpOrbPacket) },
                    { 0x02, typeof(ServerSpawnGlobalEntityPacket) },
                    { 0x03, typeof(ServerSpawnMobPacket) },
                    { 0x04, typeof(ServerSpawnPaintingPacket) },
                    { 0x05, typeof(ServerSpawnPlayerPacket) },
                    { 0x06, typeof(ServerEntityAnimationPacket) },
                    { 0x07, typeof(ServerStatisticsPacket) },
                    { 0x08, typeof(ServerBlockBreakAnimPacket) },
                    { 0x09, typeof(ServerUpdateTileEntityPacket) },
                    { 0x0A, typeof(ServerBlockValuePacket) },
                    { 0x0B, typeof(ServerBlockChangePacket) },
                    { 0x0C, typeof(ServerBossBarPacket) },
                    { 0x0D, typeof(ServerDifficultyPacket) },
                    { 0x0E, typeof(ServerTabCompletePacket) },
                    { 0x0F, typeof(ServerChatMessagePacket) },
                    { 0x10, typeof(ServerMultiBlockChangePacket) },
                    { 0x11, typeof(ServerConfirmTransactionPacket) },
                    { 0x12, typeof(ServerCloseWindowPacket) },
                    { 0x13, typeof(ServerOpenWindowPacket) },
                    { 0x14, typeof(ServerWindowItemsPacket) },
                    { 0x15, typeof(ServerWindowPropertyPacket) },
                    { 0x16, typeof(ServerSetSlotPacket) },
                    { 0x17, typeof(ServerSetCooldownPacket) },
                    { 0x18, typeof(ServerPluginMessagePacket) },
                    { 0x19, typeof(ServerPlaySoundPacket) },
                    { 0x1A, typeof(ServerDisconnectPacket) },
                    { 0x1B, typeof(ServerEntityStatusPacket) },
                    { 0x1C, typeof(ServerExplosionPacket) },
                    { 0x1D, typeof(ServerUnloadChunkPacket) },
                    { 0x1E, typeof(ServerNotifyClientPacket) },
                    { 0x1F, typeof(ServerKeepAlivePacket) },
                    { 0x20, typeof(ServerChunkDataPacket) },
                    { 0x21, typeof(ServerPlayEffectPacket) },
                    { 0x22, typeof(ServerSpawnParticlePacket) },
                    { 0x23, typeof(ServerJoinGamePacket) },
                    { 0x24, typeof(ServerMapDataPacket) },
                    { 0x25, typeof(ServerEntityMovementPacket) },
                    { 0x26, typeof(ServerEntityPositionPacket) },
                    { 0x27, typeof(ServerEntityPositionRotationPacket) },
                    { 0x28, typeof(ServerEntityRotationPacket) },
                    { 0x29, typeof(ServerVehicleMovePacket) },
                    { 0x2A, typeof(ServerOpenTileEntityEditorPacket) },
                    { 0x2B, typeof(ServerPreparedCraftingGridPacket) },
                    { 0x2C, typeof(ServerPlayerAbilitiesPacket) },
                    { 0x2D, typeof(ServerCombatPacket) },
                    { 0x2E, typeof(ServerPlayerListEntryPacket) },
                    { 0x2F, typeof(ServerPlayerPositionRotationPacket) },
                    { 0x30, typeof(ServerPlayerUseBedPacket) },
                    { 0x31, typeof(ServerUnlockRecipesPacket) },
                    { 0x32, typeof(ServerEntityDestroyPacket) },
                    { 0x33, typeof(ServerEntityRemoveEffectPacket) },
                    { 0x34, typeof(ServerResourcePackSendPacket) },
                    { 0x35, typeof(ServerRespawnPacket) },
                    { 0x36, typeof(ServerEntityHeadLookPacket) },
                    { 0x37, typeof(ServerAdvancementTabPacket) },
                    { 0x38, typeof(ServerWorldBorderPacket) },
                    { 0x39, typeof(ServerSwitchCameraPacket) },
                    { 0x3A, typeof(ServerPlayerChangeHeldItemPacket) },
                    { 0x3B, typeof(ServerDisplayScoreboardPacket) },
                    { 0x3C, typeof(ServerEntityMetadataPacket) },
                    { 0x3D, typeof(ServerEntityAttachPacket) },
                    { 0x3E, typeof(ServerEntityVelocityPacket) },
                    { 0x3F, typeof(ServerEntityEquipmentPacket) },
                    { 0x40, typeof(ServerPlayerSetExperiencePacket) },
                    { 0x41, typeof(ServerPlayerHealthPacket) },
                    { 0x42, typeof(ServerScoreboardObjectivePacket) },
                    { 0x43, typeof(ServerEntitySetPassengersPacket) },
                    { 0x44, typeof(ServerTeamPacket) },
                    { 0x45, typeof(ServerUpdateScorePacket) },
                    { 0x46, typeof(ServerSpawnPositionPacket) },
                    { 0x47, typeof(ServerUpdateTimePacket) },
                    { 0x48, typeof(ServerTitlePacket) },
                    { 0x49, typeof(ServerPlayBuiltinSoundPacket) },
                    { 0x4A, typeof(ServerPlayerListDataPacket) },
                    { 0x4B, typeof(ServerEntityCollectItemPacket) },
                    { 0x4C, typeof(ServerEntityTeleportPacket) },
                    { 0x4D, typeof(ServerAdvancementsPacket) },
                    { 0x4E, typeof(ServerEntityPropertiesPacket) },
                    { 0x4F, typeof(ServerEntityEffectPacket) },

                };
            }
            return (outPackets, inPackets);
        }
    }
}