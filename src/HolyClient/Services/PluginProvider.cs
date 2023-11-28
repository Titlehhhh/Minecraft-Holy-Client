using DynamicData;
using HolyClient.Core.Infrastructure;
using HolyClient.Models.ManagingExtensions;
using Splat;
using System;
using System.Linq;

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
				.OnItemAdded(CreatePluginSources)
				.OnItemRemoved(OnRemovedAssembly)
				.Subscribe();

		}

		private void OnRemovedAssembly(IAssemblyFile assembly)
		{
			var keys = assembly.StressTestPlugins
				.Select(x => new PluginTypeReference(assembly.Name, x.FullName));


			_stressTestPlugins.RemoveKeys(keys);
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
		public IObservableCache<IPluginSource, PluginTypeReference> AvailableStressTestPlugins => _stressTestPlugins;
	}
}