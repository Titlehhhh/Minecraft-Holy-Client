

using Microsoft.IO;

namespace McProtoNet
{
	public static class StaticResources
	{
		static StaticResources()
		{

		}

		public static readonly RecyclableMemoryStreamManager MSmanager = new RecyclableMemoryStreamManager();
	}
}
