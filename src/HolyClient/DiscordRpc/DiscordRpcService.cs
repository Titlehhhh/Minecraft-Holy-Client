using System.Threading.Tasks;
using DiscordRPC;

namespace HolyClient.DiscordRpc;

public class DiscordRpcService
{
    private DiscordRpcClient discordRpcClient;

    public DiscordRpcService()
    {
        discordRpcClient = new DiscordRpcClient("1289620437197656235");
        discordRpcClient.SetPresence(new RichPresence()
        {
            Details = "A Basic Example",
            State = "In Settings",
            Timestamps = Timestamps.FromTimeSpan(10),
            Buttons = new Button[]
            {
                new Button() { Label = "Download", Url = "https://github.com/Titlehhhh/Minecraft-Holy-Client" }
            }
        });
    }

    public void Start()
    {
        discordRpcClient.Initialize();
    }

    public void Stop()
    {
        discordRpcClient.Dispose();
    }
}