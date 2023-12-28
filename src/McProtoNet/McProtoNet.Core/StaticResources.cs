

using Microsoft.IO;

namespace McProtoNet
{
	public static class StaticResources
	{
		public static readonly RecyclableMemoryStreamManager MSmanager = new RecyclableMemoryStreamManager();
	}
}
