using HolyClient.AppState;
using HolyClient.Core.Infrastructure;
using HolyClient.StressTest;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;

namespace HolyClient.ViewModels;

public class StressTestPluginViewModel : ReactiveObject, IDisposable
{
	[Reactive]
	public bool IsInstalled { get; private set; }

	public ICommand InstallCommand { get; }

	public string Name { get; set; }
	public string? Assembly { get; set; }
	public string? Authors { get; set; }
	public string? Description { get; set; }

	private IDisposable? _cleanUp;

	public StressTestPluginViewModel(IPluginSource pluginSource, IStressTestProfile stressTest)
	{
		Name = pluginSource.Metadata.Title ?? pluginSource.Reference.FullName;
		Assembly = pluginSource.Reference.AssemblyName;
		Authors = pluginSource.Metadata.Author;
		Description = pluginSource.Metadata.Description;

		CompositeDisposable d = new();

		var canExecute = this.WhenAnyValue(x => x.IsInstalled).Select(x => !x);

		this.IsInstalled = stressTest.BehaviorRef == pluginSource.Reference;

		stressTest.WhenAnyValue(x => x.BehaviorRef)
			.Subscribe(x =>
			{
				this.IsInstalled = x == pluginSource.Reference;
			})
			.DisposeWith(d);

		InstallCommand = ReactiveCommand.Create(() =>
		{
			stressTest.SetBehavior(pluginSource);


		}, canExecute: canExecute).DisposeWith(d);

		_cleanUp = d;
	}

	public void Dispose()
	{
		Interlocked.Exchange(ref _cleanUp, null)?.Dispose();
	}
}