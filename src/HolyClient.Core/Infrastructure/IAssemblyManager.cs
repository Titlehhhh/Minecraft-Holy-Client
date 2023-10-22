using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyClient.Core.Infrastructure
{

	public interface IAssemblyManager
	{
		IConnectableCache<IAssemblyFile,string> Assemblies { get; }

		IObservable<IAssemblyFile> AssemblyFileUpdated { get; }

		Task AddReference(string path);
		Task Initialization();
	}
}
