using HolyClient.ViewModels;
using McProtoNet.MultiVersion;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HolyClient.DesignTime
{
	public class DesignBotProfileViewModel : IBotProfileViewModel
	{
		public ViewModelActivator Activator { get; }

		public Guid Id => Guid.NewGuid();

		public string Name { get; set; }

		public ICommand StartBotCommand => throw new NotImplementedException();

		public ICommand StopBotCommand => throw new NotImplementedException();

		public RoutingState Router => throw new NotImplementedException();



		public ObservableCollection<string> LogItems => throw new NotImplementedException();

		public ConsoleViewModel Console => throw new NotImplementedException();

		public string Server { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public string Nickname { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public MinecraftVersion Version { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public int SelectedTab { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public void Dispose()
		{

		}
	}
}
