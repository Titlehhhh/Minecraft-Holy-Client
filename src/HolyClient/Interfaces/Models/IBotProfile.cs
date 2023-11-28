using HolyClient.Common;
using HolyClient.Models;
using McProtoNet;
using System;
using System.ComponentModel;

namespace HolyClient.Contracts.Models
{
	[MessagePack.Union(0, typeof(BotProfile))]
	public interface IBotProfile : INotifyPropertyChanged, INotifyPropertyChanging, IDisposable
	{
		Guid Id { get; set; }
		string Name { get; set; }
		string Server { get; set; }
		string Nickname { get; set; }
		MinecraftVersion Version { get; set; }
		bool IsAuth { get; set; }
		AuthInfo Auth { get; set; }
		bool ProxyUsed { get; set; }
		ProxyInfo Proxy { get; set; }
		int SelectedTab { get; set; }


		IBotPluginStore PluignStore { get; }

		void Stop();
		void Start(Serilog.ILogger logger);


		//Task Initialization(IPluginProvider provider);


	}
}
