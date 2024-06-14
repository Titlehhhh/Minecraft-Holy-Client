using NuGet.Protocol.Core.Types;

namespace HolyClient.ViewModels;

public class NugetVersionViewModel
{
    private readonly VersionInfo _version;

    public NugetVersionViewModel(VersionInfo version)
    {
        _version = version;
    }

    public override string ToString()
    {
        return _version.Version.ToString();
    }
}