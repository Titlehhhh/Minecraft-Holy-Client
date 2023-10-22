using HolyClient.Core.StressTest;
using HolyClient.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Windows.Input;

namespace HolyClient.Commands
{
	public class StopStressTestCommand : ICommand
	{
		public event EventHandler? CanExecuteChanged;
		private readonly IScreen screen;
		private readonly IStressTest stressTest;
		public StopStressTestCommand(IScreen screen, IStressTest stressTest)
		{
			this.screen = screen;
			this.stressTest = stressTest;
		}

		public bool CanExecute(object? parameter)
		{
			return true;
		}

		public async void Execute(object? parameter)
		{
			await this.stressTest.Stop();
			await screen.Router.NavigateAndReset.Execute(new StressTestConfigurationViewModel(screen, stressTest));
		}
	}
}
