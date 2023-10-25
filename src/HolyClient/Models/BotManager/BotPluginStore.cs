using HolyClient.Contracts.Models;
using HolyClient.Contracts.Services;
using HolyClient.Core.Models.BotManager;
using HolyClient.LoadPlugins.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HolyClient.Models;

public class BotPluginStore : IBotPluginStore
{

	private MinecraftBot bot;
	private List<BotPluginReference> _stateRef;
	private Dictionary<BotPluginReference, IBotPluginCreater> _plugins = new();

	public BotPluginStore(MinecraftBot bot, List<BotPluginReference> stateRef)
	{
		this.bot = bot;
		_stateRef = stateRef;
	}



	public IEnumerable<IBotPluginCreater> Plugins => _plugins.Values;

	public void AddPlugin(IBotPluginCreater plugin)
	{
		_plugins[plugin.Token] = plugin;
		_stateRef.Add(plugin.Token);
		try
		{
			//bot.AddPlugin(plugin.Create());
		}
		catch (Exception ex)
		{
			Console.WriteLine("Не удалось добавить плагин: " + ex);
		}


	}

	public bool Contains(BotPluginReference token)
	{
		return _plugins.ContainsKey(token);
	}

	public Task Initialization(IPluginProvider pluginProvider, IEnumerable<BotPluginReference> references)
	{
		return Task.Run(() =>
		{
			foreach (var reference in references)
			{
				this._plugins.Add(reference, pluginProvider.GetPluginCreaterFromReference(reference));
			}
		});
	}

	public void RemovePlugin(BotPluginReference token)
	{
		_stateRef.Remove(token);
		_plugins.Remove(token);
	}
}



