using DynamicData;
using HolyClient.Abstractions.StressTest;
using HolyClient.Core.Infrastructure;
using HolyClient.Models.ManagingExtensions;
using HolyClient.StressTest;
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

			_stressTestPlugins.AddOrUpdate(new DefaultPluginSource());

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

	public class DefaultPluginSource : IPluginSource
	{
		
		public PluginMetadata Metadata { get; private set; } = new PluginMetadata("Titlehhhh", "Spam hello bots", "HolyClient default behavior");

		public PluginTypeReference Reference => default(PluginTypeReference);

		public T CreateInstance<T>() where T : IStressTestBehavior
		{
			IStressTestBehavior beh = new DefaultBehavior();
			return (T)beh;
		}
	}
}