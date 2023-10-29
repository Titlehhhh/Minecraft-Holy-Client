using HolyClient.AppState;
using HolyClient.Core.Infrastructure;
using System.Threading.Tasks;

namespace HolyClient.Models.ManagingExtensions
{
	public class ExtensionManager
	{
		public IAssemblyManager AssemblyManager { get; }



		private ExtensionManagerState _state;
		public ExtensionManager(ExtensionManagerState state)
		{
			_state = state;
			AssemblyManager = new AssemblyManager(state);


		}


		public async Task Initialization()
		{
			await AssemblyManager.Initialization();
		}
	}
}
