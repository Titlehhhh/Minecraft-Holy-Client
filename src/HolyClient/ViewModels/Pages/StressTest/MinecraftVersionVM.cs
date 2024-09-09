namespace HolyClient.ViewModels;

public sealed class MinecraftVersionVM
{
    public override string ToString()
    {
        return this.Version;
    }

    public static implicit operator int(MinecraftVersionVM vm)
    {
        return vm.ProtocolVersion;
    }

    public string Version { get; set; }
    public int ProtocolVersion { get; set; }

    public MinecraftVersionVM(string version, int protocolVersion)
    {
        Version = version;
        ProtocolVersion = protocolVersion;
    }

    public static MinecraftVersionVM[] GetAll()
    {
        return new MinecraftVersionVM[]
        {
            new MinecraftVersionVM("1.12.2", 340),
            new MinecraftVersionVM("1.13", 393),
            new MinecraftVersionVM("1.13.1", 401),
            new MinecraftVersionVM("1.13.2", 404),
            new MinecraftVersionVM("1.14", 477),
            new MinecraftVersionVM("1.14.1", 480),
            new MinecraftVersionVM("1.14.2", 485),
            new MinecraftVersionVM("1.14.3", 490),
            new MinecraftVersionVM("1.14.4", 498),
            new MinecraftVersionVM("1.15", 573),
            new MinecraftVersionVM("1.15.1", 575),
            new MinecraftVersionVM("1.15.2", 578),
            new MinecraftVersionVM("1.16", 735),
            new MinecraftVersionVM("1.16.1", 736),
            new MinecraftVersionVM("1.16.2", 751),
            new MinecraftVersionVM("1.16.3", 753),
            new MinecraftVersionVM("1.16.5", 754),
            new MinecraftVersionVM("1.17", 755),
            new MinecraftVersionVM("1.17.1", 756),
            new MinecraftVersionVM("1.18.1", 757),
            new MinecraftVersionVM("1.18.2", 758),
            new MinecraftVersionVM("1.19", 759),
            new MinecraftVersionVM("1.19.2", 760),
            new MinecraftVersionVM("1.19.3", 761),
            new MinecraftVersionVM("1.19.4", 762),
            new MinecraftVersionVM("1.20.1", 763),
            new MinecraftVersionVM("1.20.2", 764),
            new MinecraftVersionVM("1.20.4", 765),
            new MinecraftVersionVM("1.20.6", 766),
            new MinecraftVersionVM("1.21", 767)
        };
    }
}