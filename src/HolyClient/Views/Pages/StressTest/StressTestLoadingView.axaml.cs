using Avalonia.ReactiveUI;
using HolyClient.ViewModels;

namespace HolyClient.Views.Pages.StressTest;

public partial class StressTestLoadingView : ReactiveUserControl<StressTestLoadingViewModel>
{
	public StressTestLoadingView()
	{
		InitializeComponent();
	}
}