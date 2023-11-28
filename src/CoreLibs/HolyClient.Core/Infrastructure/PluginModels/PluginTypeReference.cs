using System.Diagnostics.CodeAnalysis;

namespace HolyClient.Core.Infrastructure
{
	public record struct PluginTypeReference(string AssemblyName, string FullName);
}
