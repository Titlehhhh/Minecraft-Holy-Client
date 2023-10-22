using McProtoNet.Protocol340.Data;

namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerPlayerListEntryPacket : MinecraftPacket
    {
        public PlayerListEntryAction Action { get; private set; }
        public PlayerListEntry[] Entries { get; private set; }



        //this.action = MagicValues.key(PlayerListEntryAction.class, in.readVarInt());
        //this.entries = new PlayerListEntry[in.readVarInt()];
        //for(int count = 0; count < this.entries.length; count++) {
        //UUID uuid = in.readUUID();
        //GameProfile profile;
        //if(this.action == PlayerListEntryAction.ADD_PLAYER) {
        //profile = new GameProfile(uuid, in.readString());
        //} else {
        //profile = new GameProfile(uuid, null);
        //}
        //
        //PlayerListEntry entry = null;
        //switch(this.action) {
        //case ADD_PLAYER:
        //int properties = in.readVarInt();
        //for(int index = 0; index < properties; index++) {
        //String propertyName = in.readString();
        //String value = in.readString();
        //String signature = null;
        //if(in.readBoolean()) {
        //signature = in.readString();
        //}
        //
        //profile.getProperties().add(new GameProfile.Property(propertyName, value, signature));
        //}
        //
        //int g = in.readVarInt();
        //GameMode gameMode = MagicValues.key(GameMode.class, g < 0 ? 0 : g);
        //int ping = in.readVarInt();
        //Message displayName = null;
        //if(in.readBoolean()) {
        //displayName = Message.fromString(in.readString());
        //}
        //
        //entry = new PlayerListEntry(profile, gameMode, ping, displayName);
        //break;
        //case UPDATE_GAMEMODE:
        //g = in.readVarInt();
        //GameMode mode = MagicValues.key(GameMode.class, g < 0 ? 0 : g);
        //entry = new PlayerListEntry(profile, mode);
        //break;
        //case UPDATE_LATENCY:
        //int png = in.readVarInt();
        //entry = new PlayerListEntry(profile, png);
        //break;
        //case UPDATE_DISPLAY_NAME:
        //Message disp = null;
        //if(in.readBoolean()) {
        //disp = Message.fromString(in.readString());
        //}
        //
        //entry = new PlayerListEntry(profile, disp);
        //case REMOVE_PLAYER:
        //entry = new PlayerListEntry(profile);
        //break;
        //}
        //
        //this.entries[count] = entry;
        //}
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

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerPlayerListEntryPacket() { }
    }

}
