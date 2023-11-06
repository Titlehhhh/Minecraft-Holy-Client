using McProtoNet;
using ReactiveUI;
using System;

namespace HolyClient.ViewModels
{
	public interface IBotProfileViewModel : IActivatableViewModel
	{
		Guid Id { get; }
		string Name { get; set; }
		string Server { get; set; }
		string Nickname { get; set; }
		MinecraftVersion Version { get; set; }

		int SelectedTab { get; set; }
		ConsoleViewModel Console { get; }




	}
}