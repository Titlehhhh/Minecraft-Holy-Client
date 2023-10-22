using Avalonia.ReactiveUI;
using HolyClient.ViewModels;

namespace HolyClient.Views;

public partial class StressTestView : ReactiveUserControl<StressTestViewModel>
{
	public StressTestView()
	{
		InitializeComponent();
	}
}