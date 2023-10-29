using DynamicData;
using HolyClient.Contracts.Services;
using HolyClient.LoadPlugins;
using HolyClient.LoadPlugins.Models;
using HolyClient.Models;
using HolyClient.Models.ManagingExtensions;
using Splat;

namespace HolyClient.Services
{
	public class PluginProvider : IPluginProvider
	{
		public PluginProvider()
		{

			var extensionManager = Locator.Current.GetService<ExtensionManager>();

		}


	}



}