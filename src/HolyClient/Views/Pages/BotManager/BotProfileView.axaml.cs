using Avalonia.ReactiveUI;
using HolyClient.ViewModels;
using ReactiveUI;

namespace HolyClient.Views;

public partial class BotProfileView : ReactiveUserControl<IBotProfileViewModel>
{
	public BotProfileView()
	{
		InitializeComponent();
		this.WhenActivated(x => { });
	}
}