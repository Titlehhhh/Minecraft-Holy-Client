using ReactiveUI;

namespace HolyClient.ViewModels
{
	public class PluginViewModel : ReactiveObject
	{


		public PluginViewModel(string fullName)
		{
			FullName = fullName;
		}

		public string FullName { get; }

	}

}
