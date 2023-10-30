using HolyClient.Localization;
using HolyClient.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;

namespace HolyClient.ViewModels
{
	public class SettingsViewModel : ReactiveObject, ISettingsViewModel, IRoutableViewModel, IActivatableViewModel
	{

		[Reactive]
		public string Language { get; set; }
		public ObservableCollection<string> AvailableLanguages => Loc.Instance.AvailableLanguages;

		public string? UrlPathSegment => "/settings";

		public IScreen HostScreen { get; }

		public ViewModelActivator Activator { get; } = new();

		public SettingsViewModel()
		{
			var state = Locator.Current.GetService<SettingsState>();

			Language = state.Language;
			this.WhenActivated(d =>
			{
				this.WhenAnyValue(x => x.Language)
				.BindTo(state, x => x.Language)
				.DisposeWith(d);

				this.WhenAnyValue(x => x.Language)
				.Subscribe(x => Loc.Instance.CurrentLanguage = x)
				.DisposeWith(d);
			});


		}



	}
}
