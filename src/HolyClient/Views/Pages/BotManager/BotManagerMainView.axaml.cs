using Avalonia.ReactiveUI;
using HolyClient.ViewModels;

namespace HolyClient.Views
{
	public partial class BotManagerMainView : ReactiveUserControl<IBotManagerViewModel>
	{
		public BotManagerMainView()
		{

			InitializeComponent();
		}
	}
}
