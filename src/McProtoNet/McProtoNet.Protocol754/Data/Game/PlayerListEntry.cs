namespace McProtoNet.Protocol754.Data
{

    public class PlayerListEntry
    {
        public GameProfile Profile { get; set; }
        public GameMode GameMode { get; set; }
        public int Ping { get; set; }
        public string? DisplayName { get; set; }
    }
}
