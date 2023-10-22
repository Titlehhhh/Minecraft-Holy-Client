using Newtonsoft.Json;

namespace McProtoNet.Utils
{
    public class ServerInfo
    {
        [JsonProperty(PropertyName = "version", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public VersionInfo TargetVersion { get; set; }

        [JsonProperty(PropertyName = "players", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public PlayerInfo Players { get; set; }

        [JsonProperty(PropertyName = "description", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public ChatMessage Description { get; set; }



        [JsonProperty(PropertyName = "favicon", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string Icon { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
            string header = $"Сервер версия {TargetVersion.StringVersion} {Players.OnlinePlayers}/{Players.MaxPlayers}";
            string desc = "Описание: \n" + Description.ToString();
            string players = "Игроки:\n" + string.Join("\n", Players.PlayerList.Select(x => x.Username));
            return header + "\n" + desc + "\n" + players;
        }
    }
}