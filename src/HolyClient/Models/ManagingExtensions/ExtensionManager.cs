using System.Threading.Tasks;
using HolyClient.AppState;
using HolyClient.Core.Infrastructure;

namespace HolyClient.Models.ManagingExtensions;

public class ExtensionManager
{
    private ExtensionManagerState _state;

    public ExtensionManager(ExtensionManagerState state)
    {
        _state = state;
        AssemblyManager = new AssemblyManager(state);
    }

    public IAssemblyManager AssemblyManager { get; }


    public async Task Initialization()
    {
        await AssemblyManager.Initialization();
    }
}