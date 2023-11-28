﻿using HolyClient.LoadPlugins.Models;
using HolyClient.Models;
using System.Collections.Generic;

namespace HolyClient.Contracts.Models
{
	public interface IBotPluginStore
	{
		IEnumerable<IBotPluginCreater> Plugins { get; }

		bool Contains(BotPluginReference token);
		void AddPlugin(IBotPluginCreater plugin);
		void RemovePlugin(BotPluginReference token);

		//	Task Initialization(IPluginProvider pluginProvider, IEnumerable<BotPluginReference> references);
	}
}

