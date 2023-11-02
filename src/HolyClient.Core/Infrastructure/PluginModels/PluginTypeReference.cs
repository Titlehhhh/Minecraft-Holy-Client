using System.Diagnostics.CodeAnalysis;

namespace HolyClient.Core.Infrastructure
{
	public struct PluginTypeReference
	{
		public string AssemblyName { get; }
		public string FullName { get; }

		public PluginTypeReference(string assemblyName, string fullName)
		{
			AssemblyName = assemblyName;
			FullName = fullName;
		}
		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			if (obj is PluginTypeReference reference)
			{
				return this.AssemblyName == reference.AssemblyName && this.FullName == reference.AssemblyName;
			}
			return false;
		}
		public override int GetHashCode()
		{
			return HashCode.Combine(AssemblyName, FullName);
		}
		public override string ToString()
		{
			return $"Assembly: {AssemblyName} FullName: {FullName}";
		}
	}
}
