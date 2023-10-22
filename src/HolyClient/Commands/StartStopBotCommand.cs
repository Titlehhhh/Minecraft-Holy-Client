using HolyClient.Contracts.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Windows.Input;

namespace HolyClient.Commands
{
	public class StartStopBotCommand : ReactiveObject, ICommand
	{

		private IBotProfile _profile;
		private Serilog.ILogger _logger;
		[Reactive]
		public bool IsActivate { get; private set; } = false;


		public StartStopBotCommand(IBotProfile profile, Serilog.ILogger logger)
		{
			_logger = logger;
			_profile = profile;
		}
		private bool _canExectute;

		public event EventHandler? CanExecuteChanged;

		private void RaiseCanExecute()
		{

		}

		public bool CanExecute(object? parameter)
		{
			return true;
		}

		public void Execute(object? parameter)
		{
			if (IsActivate)
			{
				_profile.Stop();
			}
			else
			{
				_profile.Start(this._logger);
			}
			IsActivate = !IsActivate;
		}
	}
}
