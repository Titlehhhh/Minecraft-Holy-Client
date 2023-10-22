using HolyClient.Models;
using HolyClient.SDK;

namespace HolyClient.LoadPlugins.Models
{
	public interface IBotPluginCreater
	{
		BotPluginReference Token { get; }

		string Name { get; }
		string Assembly { get; }
		string AssemblyFile { get; }

		BotPlugin Create();


	}
}
