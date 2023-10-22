using DynamicData;
using HolyClient.LoadPlugins.Models;
using HolyClient.Models;

namespace HolyClient.Contracts.Services;

public interface IPluginProvider
{
	ISourceCache<IBotPluginCreater, BotPluginReference> AvailableBotPlugins { get; }

	IBotPluginCreater GetPluginCreaterFromReference(BotPluginReference reference);
}



