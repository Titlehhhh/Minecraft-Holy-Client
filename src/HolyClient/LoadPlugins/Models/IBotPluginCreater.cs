using HolyClient.AppState;

namespace HolyClient.LoadPlugins.Models
{
	public interface IBotPluginCreater
	{
		BehaviorKey Token { get; }

		string Name { get; }
		string Assembly { get; }
		string AssemblyFile { get; }

		//BotPlugin Create();


	}
}
