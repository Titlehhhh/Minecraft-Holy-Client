using Avalonia.ReactiveUI;
using HolyClient.ViewModels;

namespace HolyClient.Views;

public partial class StressTestLoadingView : ReactiveUserControl<StressTestLoadingViewModel>
{
	public StressTestLoadingView()
	{
		InitializeComponent();
	}
}