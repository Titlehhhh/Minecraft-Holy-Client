using HolyClient.Abstractions.StressTest;
using HolyClient.Core.Infrastructure;
using System;

namespace HolyClient.Services
{
	public class PluginSource : IPluginSource
	{
		private Type type;
		private PluginTypeReference typeReference;

		public PluginSource(Type type, PluginTypeReference typeReference)
		{
			this.type = type;
			this.typeReference = typeReference;
		}

		public PluginTypeReference Reference => typeReference;

		public T CreateInstance<T>() where T : IStressTestBehavior
		{
			var plugin = Activator.CreateInstance(type);
			return (T)plugin;
		}
	}
}