using DynamicData;
using HolyClient.Contracts.Services;
using HolyClient.Core.Infrastructure;
using HolyClient.LoadPlugins;
using HolyClient.LoadPlugins.Models;
using HolyClient.Models;
using HolyClient.Models.ManagingExtensions;
using Splat;
using System;

namespace HolyClient.Services
{
	public class PluginProvider : IPluginProvider
	{
		public PluginProvider()
		{

			var extensionManager = Locator.Current.GetService<ExtensionManager>();

			foreach (var assembly in extensionManager.AssemblyManager.Assemblies.Items)
			{
				CreatePluginSources(assembly);
			}

			extensionManager.AssemblyManager.Assemblies
				.Connect()				
				.OnItemAdded(CreatePluginSources);

		}

		private void CreatePluginSources(IAssemblyFile assembly)
		{
			foreach (Type stressTestPlugin in assembly.StressTestPlugins)
			{
				PluginTypeReference reference = new(assembly.Name, stressTestPlugin.FullName);

				var source = new PluginSource(stressTestPlugin, reference);

				_stressTestPlugins.AddOrUpdate(source);

			}
		}
		private SourceCache<IPluginSource, PluginTypeReference> _stressTestPlugins = new(x => x.Reference);
		public IConnectableCache<IPluginSource, PluginTypeReference> AvailableStressTestPlugins { get; private set; }
	}
}