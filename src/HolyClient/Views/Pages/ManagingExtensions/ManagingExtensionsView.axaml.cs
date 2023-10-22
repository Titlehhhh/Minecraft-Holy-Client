using Avalonia.ReactiveUI;
using HolyClient.ViewModels;
using ReactiveUI;

namespace HolyClient.Views;

public partial class ManagingExtensionsView : ReactiveUserControl<ManagingExtensionsViewModel>
{
	public ManagingExtensionsView()
	{
		InitializeComponent();
		this.WhenActivated(d =>
		{

		});

	}
}