using HolyClient.AppState;
using HolyClient.Models;
using System;

namespace HolyClient.LoadPlugins.Models
{
	public class BotPluginCreater : IBotPluginCreater
	{

		private Type _type;
		public BotPluginCreater(Type type, string assembly, string assemblyFile)
		{
			_type = type;
			Name = _type.FullName;
			Assembly = assembly;
			AssemblyFile = assemblyFile;
		}

		public string Name { get; private set; }

		public string Assembly { get; private set; }

		public string AssemblyFile { get; private set; }



		public BehaviorKey Token => new BehaviorKey(this.Name, this.Assembly);

		//public BotPlugin Create()
		//{
		//	//return (BotPlugin)Activator.CreateInstance(_type);
		//}
	}
}
