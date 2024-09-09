using System.Reactive;
using McProtoNet.Abstractions;
using McProtoNet.Serialization;

namespace McProtoNet.MultiVersionProtocol;

[Experimental]
public sealed class MultiProtocol : ProtocolBase
{
    private static readonly byte[] bitset = new byte[3];

    public MultiProtocol(IPacketBroker client) : base(client)
    {
        //SupportedVersion = 755;
    }

    protected override void OnPacketReceived(InputPacket packet)
    {
        
        base.OnPacketReceived(packet);
    }

    public ValueTask SendChatPacket(string message)
    {
        scoped var writer = new MinecraftPrimitiveWriterSlim();
        int id = ProtocolVersion switch
        {
            340 => 0x02,
            >= 341 and <= 392 => 0x03,
            >= 393 and <= 404 => 0x02,
            >= 405 and <= 758 => 0x03,
            759 => 0x04,
            >= 760 and <= 765 => 0x05,
            >= 766 and <= 767 => 0x06,
            _ => throw new Exception("Unknown protocol version")
        };
        writer.WriteVarInt(id); // Packet Id
        writer.WriteString(message);
        if (ProtocolVersion >= 759)
        {
            long timeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            writer.WriteSignedLong(timeStamp);
            writer.WriteSignedLong(0);
            if (ProtocolVersion < 761)
            {
                writer.WriteVarInt(0);
            }
            else
            {
                writer.WriteBoolean(false);
            }

            if (ProtocolVersion <= 760)
                writer.WriteBoolean(false);

            switch (ProtocolVersion)
            {
                case >= 761:
                    writer.WriteVarInt(0);
                    writer.WriteBuffer(bitset);
                    break;
                case 760:
                    writer.WriteVarInt(0);
                    writer.WriteBoolean(false);
                    break;
            }
        }

        return base.SendPacketCore(writer.GetWrittenMemory());
    }


    public ValueTask SendKeepAlive(long id)
    {
        scoped var writer = new MinecraftPrimitiveWriterSlim();
        int packetId = ProtocolVersion switch
        {
            340 => 0x0B,
            >= 341 and <= 392 => 0x0F,
            >= 393 and <= 404 => 0x0E,
            >= 405 and <= 476 => 0x10,
            >= 477 and <= 578 => 0x0F,
            >= 579 and <= 754 => 0x10,
            >= 755 and <= 758 => 0x0F,
            759 => 0x11,
            760 => 0x12,
            761 => 0x11,
            >= 762 and <= 763 => 0x12,
            764 => 0x14,
            765 => 0x15,
            >= 766 and <= 767 => 0x18,
            _ => throw new Exception("Unknown protocol version")
        };
        writer.WriteVarInt(packetId); // Packet Id
        writer.WriteSignedLong(id);
        return base.SendPacketCore(writer.GetWrittenMemory());
    }

    public static List<string> ServerboundPlayPackets(int protocolVersion)
    {
        List<string> result = new();

        result.Add("ServerboundAcceptTeleportationPacket");
        if (protocolVersion > 340) /* > 1.12.2 */
        {
            result.Add("ServerboundBlockEntityTagQueryPacket");
        }

        if (protocolVersion < 393) /* < 1.13 */
        {
            result.Add("ServerboundCommandSuggestionPacket");
        }

        if (protocolVersion > 404) /* > 1.13.2 */
        {
            result.Add("ServerboundChangeDifficultyPacket");
        }

        if (protocolVersion > 759) /* > 1.19 */
        {
            result.Add("ServerboundChatAckPacket");
        }

        if (protocolVersion > 758) /* > 1.18.2 */
        {
            result.Add("ServerboundChatCommandPacket");
        }

        if (protocolVersion > 765) /* > 1.20.4 */
        {
            result.Add("ServerboundChatCommandSignedPacket");
        }

        result.Add("ServerboundChatPacket");
        if (protocolVersion > 758 && protocolVersion < 761)
        {
            result.Add("ServerboundChatPreviewPacket");
        }

        if (protocolVersion > 761) /* > 1.19.3 */
        {
            result.Add("ServerboundChatSessionUpdatePacket");
        }

        if (protocolVersion > 763) /* > 1.20.1 */
        {
            result.Add("ServerboundChunkBatchReceivedPacket");
        }

        result.Add("ServerboundClientCommandPacket");
        result.Add("ServerboundClientInformationPacket");
        if (protocolVersion > 340) /* > 1.12.2 */
        {
            result.Add("ServerboundCommandSuggestionPacket");
        }

        if (protocolVersion > 763) /* > 1.20.1 */
        {
            result.Add("ServerboundConfigurationAcknowledgedPacket");
        }

        if (protocolVersion < 755) /* < 1.17 */
        {
            result.Add("ServerboundContainerAckPacket");
        }

        if (protocolVersion > 404) /* > 1.13.2 */
        {
            result.Add("ServerboundContainerButtonClickPacket");
        }

        if (protocolVersion < 477) /* < 1.14 */
        {
            result.Add("ServerboundEnchantItemPacket");
        }

        result.Add("ServerboundContainerClickPacket");
        result.Add("ServerboundContainerClosePacket");
        if (protocolVersion > 764) /* > 1.20.2 */
        {
            result.Add("ServerboundContainerSlotStateChangedPacket");
        }

        if (protocolVersion > 765) /* > 1.20.4 */
        {
            result.Add("ServerboundCookieResponsePacket");
        }

        result.Add("ServerboundCustomPayloadPacket");
        if (protocolVersion > 765) /* > 1.20.4 */
        {
            result.Add("ServerboundDebugSampleSubscriptionPacket");
        }

        if (protocolVersion > 340) /* > 1.12.2 */
        {
            result.Add("ServerboundEditBookPacket");
            result.Add("ServerboundEntityTagQueryPacket");
        }

        result.Add("ServerboundInteractPacket");
        if (protocolVersion > 578) /* > 1.15.2 */
        {
            result.Add("ServerboundJigsawGeneratePacket");
        }

        result.Add("ServerboundKeepAlivePacket");
        if (protocolVersion > 404) /* > 1.13.2 */
        {
            result.Add("ServerboundLockDifficultyPacket");
        }

        if (protocolVersion < 477) /* < 1.14 */
        {
            result.Add("ServerboundMovePlayerPacket");
        }

        result.Add("ServerboundMovePlayerPacketPos");
        result.Add("ServerboundMovePlayerPacketPosRot");
        result.Add("ServerboundMovePlayerPacketRot");
        if (protocolVersion > 404 && protocolVersion < 755)
        {
            result.Add("ServerboundMovePlayerPacket");
        }

        if (protocolVersion > 754) /* > 1.16.5 */
        {
            result.Add("ServerboundMovePlayerPacketStatusOnly");
        }

        result.Add("ServerboundMoveVehiclePacket");
        result.Add("ServerboundPaddleBoatPacket");
        if (protocolVersion > 340) /* > 1.12.2 */
        {
            result.Add("ServerboundPickItemPacket");
        }

        if (protocolVersion > 763) /* > 1.20.1 */
        {
            result.Add("ServerboundPingRequestPacket");
        }

        result.Add("ServerboundPlaceRecipePacket");
        result.Add("ServerboundPlayerAbilitiesPacket");
        result.Add("ServerboundPlayerActionPacket");
        result.Add("ServerboundPlayerCommandPacket");
        result.Add("ServerboundPlayerInputPacket");
        if (protocolVersion > 754) /* > 1.16.5 */
        {
            result.Add("ServerboundPongPacket");
        }

        if (protocolVersion > 760 && protocolVersion < 762)
        {
            result.Add("ServerboundChatSessionUpdatePacket");
        }

        if (protocolVersion < 751) /* < 1.16.2 */
        {
            result.Add("ServerboundRecipeBookUpdatePacket");
        }

        if (protocolVersion > 736) /* > 1.16.1 */
        {
            result.Add("ServerboundRecipeBookChangeSettingsPacket");
            result.Add("ServerboundRecipeBookSeenRecipePacket");
        }

        if (protocolVersion > 340) /* > 1.12.2 */
        {
            result.Add("ServerboundRenameItemPacket");
        }

        result.Add("ServerboundResourcePackPacket");
        result.Add("ServerboundSeenAdvancementsPacket");
        if (protocolVersion > 340) /* > 1.12.2 */
        {
            result.Add("ServerboundSelectTradePacket");
            result.Add("ServerboundSetBeaconPacket");
        }

        result.Add("ServerboundSetCarriedItemPacket");
        if (protocolVersion > 340) /* > 1.12.2 */
        {
            result.Add("ServerboundSetCommandBlockPacket");
            result.Add("ServerboundSetCommandMinecartPacket");
        }

        result.Add("ServerboundSetCreativeModeSlotPacket");
        if (protocolVersion > 404) /* > 1.13.2 */
        {
            result.Add("ServerboundSetJigsawBlockPacket");
        }

        if (protocolVersion > 340) /* > 1.12.2 */
        {
            result.Add("ServerboundSetStructureBlockPacket");
        }

        result.Add("ServerboundSignUpdatePacket");
        result.Add("ServerboundSwingPacket");
        result.Add("ServerboundTeleportToEntityPacket");
        result.Add("ServerboundUseItemOnPacket");
        result.Add("ServerboundUseItemPacket");

        return result;
    }
}