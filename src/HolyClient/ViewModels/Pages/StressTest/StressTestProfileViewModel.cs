using HolyClient.AppState;
using ReactiveUI;
using ReactiveUI.Validation.Helpers;
using System;

namespace HolyClient.ViewModels;

public sealed class StressTestProfileViewModel : ReactiveValidationObject, IRoutableViewModel, IActivatableViewModel
{
	public Guid Id { get; private set; }

	public ViewModelActivator Activator { get; } = new();

	public string? UrlPathSegment => throw new NotImplementedException();

	public IScreen HostScreen { get; private set; }

	public string Name => "ASdasd";

	public StressTestProfileViewModel(StressTestProfileState state)
	{
		Id = state.Id;
	}


}
