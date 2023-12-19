using Avalonia.Controls.Notifications;
using HolyClient.Services;
using HolyClient.StressTest;
using QuickProxyNet;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Windows.Input;

namespace HolyClient.ViewModels.Pages.StressTest.Dialogs
{
	public sealed class ImportProxyViewModel : ReactiveObject
	{


		[Reactive]
		public string FilePath
		{
			get;
			set;
		}



		[Reactive]
		public ProxyType Type { get; set; }

		public IEnumerable<ProxyType> Types { get; } = Enum.GetValues<ProxyType>();

		public ICommand ImportCommand { get; }
		public Interaction<Unit, Unit> ImportingTaskDialog { get; } = new();

		public ImportProxyViewModel(IStressTest state)
		{
			var proxyLoader = Locator.Current.GetService<IProxyLoaderService>();
			var notifi = Locator.Current.GetService<INotificationManager>();

			ImportCommand = ReactiveCommand.CreateFromTask(async () =>
			{
				try
				{
					await using (var stream = File.Open(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						//var count = await proxyLoader.Load(stream, Type, state.Proxies);
						var count = 0;
						notifi.Show(new Avalonia.Controls.Notifications.Notification("Прокси", $"Загружено {count} {this.Type} прокси.", type: NotificationType.Success));
					}
				}
				catch (Exception ex)
				{
					notifi.Show(
						new Avalonia.Controls.Notifications.Notification(
							"Прокси",
							$"Ошибка загрузки прокси: {ex.Message}",
							type: NotificationType.Error));
				}
			});

		}
	}
}
