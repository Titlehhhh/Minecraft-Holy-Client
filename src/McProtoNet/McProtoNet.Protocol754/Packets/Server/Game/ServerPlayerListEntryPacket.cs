using McProtoNet.Protocol754.Data;

namespace McProtoNet.Protocol754.Packets.Server
{


    public sealed class ServerPlayerListEntryPacket : MinecraftPacket
    {
        public PlayerListEntryAction Action { get; set; }
        public PlayerListEntry[] Entries { get; set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            Action = (PlayerListEntryAction)stream.ReadVarInt();
            Entries = new PlayerListEntry[stream.ReadVarInt()];

            for (int count = 0; count < Entries.Length; count++)
            {
                Guid uuid = stream.ReadUUID();
                GameProfile profile;
                if (this.Action == PlayerListEntryAction.ADD_PLAYER)
                {
                    profile = new GameProfile(uuid, stream.ReadString());
                }
                else
                {
                    profile = new GameProfile(uuid, null);
                }

                PlayerListEntry entry = null;


                string displayName = null;
                int ping = 0;


                switch (this.Action)
                {
                    case PlayerListEntryAction.ADD_PLAYER:
                        int properties = stream.ReadVarInt();
                        for (int index = 0; index < properties; index++)
                        {
                            stream.ReadString();
                            stream.ReadString();
                            if (stream.ReadBoolean())
                                stream.ReadString();

                        }
                        int rawGameMode = stream.ReadVarInt();

                        GameMode gameMode = (GameMode)Math.Max(rawGameMode, 0);
                        ping = stream.ReadVarInt();

                        if (stream.ReadBoolean())
                            displayName = stream.ReadString();
                        entry = new PlayerListEntry()
                        {
                            Profile = profile,
                            DisplayName = displayName,
                            Ping = ping,
                            GameMode = gameMode
                        };


                        break;

                    case PlayerListEntryAction.UPDATE_GAMEMODE:

                        rawGameMode = stream.ReadVarInt();
                        GameMode mode = (GameMode)Math.Max(rawGameMode, 0);

                        entry = new PlayerListEntry()
                        {
                            Profile = profile,
                            GameMode = mode
                        };

                        break;


                    case PlayerListEntryAction.UPDATE_LATENCY:
                        ping = stream.ReadVarInt();
                        entry = new PlayerListEntry()
                        {
                            Profile = profile,
                            Ping = ping
                        };
                        break;

                    case PlayerListEntryAction.UPDATE_DISPLAY_NAME:
                        displayName = null;
                        if (stream.ReadBoolean())
                        {
                            displayName = stream.ReadString();
                        }
                        entry = new PlayerListEntry()
                        {
                            Profile = profile,
                            DisplayName = displayName
                        };
                        break;

                    case PlayerListEntryAction.REMOVE_PLAYER:
                        entry = new PlayerListEntry()
                        {
                            Profile = profile
                        };
                        break;
                }

                Entries[count] = entry;
            }
        }
    }
}










