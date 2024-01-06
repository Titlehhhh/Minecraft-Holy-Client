using DynamicData;
using HolyClient.Abstractions.StressTest;
using HolyClient.Common;
using HolyClient.Core.Infrastructure;
using HolyClient.StressTest;
using HolyClient.ViewModels.Pages.StressTest.Dialogs;
using McProtoNet;
using NuGet.Configuration;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using ReactiveUI.Validation.States;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;

namespace HolyClient.ViewModels;

public class StressTestConfigurationViewModel : ReactiveValidationObject, IRoutableViewModel, IActivatableViewModel
{

	

	public string? UrlPathSegment => throw new NotImplementedException();
	public IScreen HostScreen { get; }
	public ViewModelActivator Activator { get; } = new();


	



	private SelectImportSourceProxyViewModel _selectProxyImportSourceViewModel = new();
	public StressTestConfigurationViewModel(IScreen hostScreen, IStressTestProfile state)
	{


		
	}



}
