using MessagePack;
using ReactiveUI;
using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;

namespace HolyClient.Services
{

	public class DefaultSuspensionDriver<TAppState> : ISuspensionDriver
	where TAppState : class
	{
		private IFormatterResolver resolver;
		private MessagePackSerializerOptions options;
		public DefaultSuspensionDriver()
		{
			resolver = MessagePack.Resolvers.CompositeResolver.Create(
			   MessagePack.Resolvers.BuiltinResolver.Instance,
			   MessagePack.Resolvers.AttributeFormatterResolver.Instance,

			   // replace enum resolver
			   MessagePack.Resolvers.DynamicEnumAsStringResolver.Instance,

			   MessagePack.Resolvers.DynamicGenericResolver.Instance,
			   MessagePack.Resolvers.DynamicUnionResolver.Instance,
			   MessagePack.Resolvers.DynamicObjectResolver.Instance,

			   MessagePack.Resolvers.PrimitiveObjectResolver.Instance,

			   // final fallback(last priority)
			   MessagePack.Resolvers.DynamicContractlessObjectResolver.Instance
		   );
			options = MessagePackSerializerOptions.Standard.WithResolver(resolver);
		}

		private string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

		private string dataFileName = "appstate.dat";

		public IObservable<Unit> InvalidateState() => Observable.Empty<Unit>();

		public IObservable<object> LoadState()
		{

			try
			{
				string path = Path.Combine(appData, App.AppName);
				if (Directory.Exists(path))
				{

					path = Path.Combine(path, dataFileName);

					if (File.Exists(path))
					{
						var data = File.ReadAllBytes(path);

						var state = MessagePackSerializer.Deserialize<TAppState>(data);
						if (state is null)
						{
							Console.WriteLine("State null");
							throw new NullReferenceException("App State is null");
						}
						Console.WriteLine("Состояние упешно загружено");
						return Observable.Return(state);

					}
					throw new FileNotFoundException($"Не найден файл с состоянием на пути: {path}");
				}
				throw new DirectoryNotFoundException($"Не найдена папка приложения: {path}");
			}
			catch (Exception ex)
			{
				Console.WriteLine("SuspendErr: " + ex);
				return Observable.Throw<object>(ex);
			}


		}

		public IObservable<Unit> SaveState(object state)
		{
			string path = Path.Combine(appData, App.AppName);
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			path = Path.Combine(path, dataFileName);






			var data = MessagePackSerializer.Serialize<TAppState>((TAppState)state);

			using (var sw = new StreamWriter(path))
			{
				sw.BaseStream.Write(data);
			}

			return Observable.Return(Unit.Default);

		}
	}

}