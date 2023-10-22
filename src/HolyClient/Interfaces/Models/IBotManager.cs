using DynamicData;
using HolyClient.Models;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace HolyClient.Contracts.Models
{

	public interface IExtensionManager
	{

	}



	[MessagePack.Union(0, typeof(BotManager))]
	public interface IBotManager : INotifyPropertyChanged, INotifyPropertyChanging
	{
		IConnectableCache<IBotProfile, Guid> Profiles { get; }

		void RemoveBot(Guid id);
		IBotProfile CreateAndAddBot();

		Guid SelectedProfile { get; set; }

		Task Initialization();
	}
}

