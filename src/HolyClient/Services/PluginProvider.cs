using DynamicData;
using HolyClient.Contracts.Services;
using HolyClient.LoadPlugins;
using HolyClient.LoadPlugins.Models;
using HolyClient.Models;
using HolyClient.Models.ManagingExtensions;
using HolyClient.SDK;
using Splat;

namespace HolyClient.Services
{
	public class PluginProvider : IPluginProvider
	{
		private SourceCache<IBotPluginCreater, BotPluginReference> _availableBotPlugins = new(x => x.Token);
		public ISourceCache<IBotPluginCreater, BotPluginReference> AvailableBotPlugins => _availableBotPlugins;

		public PluginProvider()
		{

			var extensionManager = Locator.Current.GetService<ExtensionManager>();

		}

		private void OnAddedAssembly(AssemblyWrapper assembly)
		{
			if (assembly.CurrentState == PluginState.Loaded)
			{

			}
			else
			{

			}
		}
		private void OnRemovedAssembly(AssemblyWrapper assembly)
		{

		}

		public IBotPluginCreater GetPluginCreaterFromReference(BotPluginReference reference)
		{
			var pl = this.AvailableBotPlugins.Lookup(reference);
			if (pl.HasValue)
			{
				return pl.Value;
			}
			return new NotLoadedBotPluginCreater(reference);
		}
	}

	public class NotLoadedBotPluginCreater : IBotPluginCreater
	{
		private BotPluginReference _ref;

		public NotLoadedBotPluginCreater(BotPluginReference @ref)
		{
			_ref = @ref;
		}

		public BotPluginReference Token => _ref;

		public string Name => _ref.Name;

		public string Assembly => _ref.Assembly;

		public string AssemblyFile => "Unkown";



		public BotPlugin Create()
		{
			return null;
		}
	}



}