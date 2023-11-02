using HolyClient.Abstractions.StressTest;
using HolyClient.Core.Infrastructure;
using HolyClient.SDK.Attributes;
using System;
using System.Reflection;

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

			string? author = type.GetCustomAttribute<PluginAuthorAttribute>()?.Author;
			string? title = type.GetCustomAttribute<PluginTitleAttribute>()?.Title;
			string? description = type.GetCustomAttribute<PluginDescriptionAttribute>()?.Description;

			Metadata = new PluginMetadata(author, description, title);
		}

		public PluginTypeReference Reference => typeReference;

		public PluginMetadata Metadata { get; private set; }

		public T CreateInstance<T>() where T : IStressTestBehavior
		{
			var plugin = Activator.CreateInstance(type);
			return (T)plugin;
		}
	}
}