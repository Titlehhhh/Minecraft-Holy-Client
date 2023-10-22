using Avalonia.Threading;
using ReactiveUI;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HolyClient.ViewModels
{
	public class ConsoleViewModel : ReactiveObject, ILogger
	{


		public void Clear() => LogItems.Clear();
		public ICommand SendCommand { get; }
		public string MessageText { get; set; }
		private ObservableCollection<string> _log = new();
		public ObservableCollection<string> LogItems => _log;

		public async void Write(LogEvent logEvent)
		{

			string level = logEvent.Level switch
			{
				(LogEventLevel.Debug) => "DEBUG",
				(LogEventLevel.Error) => "ERROR",
				(LogEventLevel.Fatal) => "FATAL",
				(LogEventLevel.Information) => "INFO",
				(LogEventLevel.Verbose) => "VERB",
				(LogEventLevel.Warning) => "WARN"
			};

			string date = logEvent.Timestamp.ToString("T");



			await Dispatcher.UIThread.InvokeAsync(() =>
			   {
				   _log.Add($"[{date}] [{level}] {logEvent.MessageTemplate.Text} {FormatException(logEvent.Exception)}");
			   });
		}
		private string FormatException(Exception? ex)
		{
			if (ex is null)
				return null;

			return $"\n{ex.Message}\n{ex.StackTrace}";

		}
	}

}
