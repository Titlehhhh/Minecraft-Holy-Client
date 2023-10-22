using System;

namespace HolyClient.ViewModels
{
	public interface IAssemblyViewModel
	{

		string Description { get; }
		Version Version { get; }

		string Path { get; }

		string Name { get; }

	}
}