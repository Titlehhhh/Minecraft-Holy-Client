using HolyClient.LoadPlugins.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace HolyClient.LoadPlugins
{
	public class AssemblyWrapper : ReactiveObject
	{

		[Reactive]
		public PluginState CurrentState { get; private set; }

		[Reactive]
		public Assembly CurrentAssembly { get; private set; }

		public string FullPath => _path;
		[Reactive]
		public string Name { get; private set; }

		private readonly string _path;
		private ManagedLoadContext? _loadContext;

		private AssemblyLoadContextBuilder _builder;

		private SemaphoreSlim _lock = new(1, 1);
		public AssemblyWrapper(string path)
		{
			_path = path;
			Name = System.IO.Path.GetFileNameWithoutExtension(_path);
			_builder = CreateLoadContextBuilder(new PluginConfig(_path));
		}

		public Task Load()
		{
			return Task.Run(async () =>
			{
				try
				{
					await _lock.WaitAsync();
					CurrentState = PluginState.Loading;
					if (_loadContext is not null)
					{
						_loadContext.Unload();
						_loadContext = null;
					}
					_loadContext = (ManagedLoadContext)_builder.Build();
					var assembly = _loadContext.LoadAssemblyFromFilePath(_path);
					if (assembly is null)
					{
						CurrentState = PluginState.Errored;
						return;
					}

					CurrentAssembly = assembly;
					Name = CurrentAssembly.GetName().Name;
					CurrentState = PluginState.Loaded;
				}
				catch (Exception ex)
				{

					CurrentState = PluginState.Errored;
				}
				finally
				{

					_lock.Release();
				}
			});
		}
		public async Task Reload()
		{
			await UnLoad();
			await Load();
		}
		public Task UnLoad()
		{
			return Task.Run(async () =>
			{
				try
				{
					await _lock.WaitAsync();
					CurrentState = PluginState.Unloading;
					if (_loadContext is null)
						return;

					_loadContext.Unload();

					_loadContext = null;
					CurrentState = PluginState.Unloaded;
				}
				catch
				{
					CurrentState = PluginState.Errored;
				}
				finally
				{

					_lock.Release();
				}
			});
		}




		private static AssemblyLoadContextBuilder CreateLoadContextBuilder(PluginConfig config)
		{
			var builder = new AssemblyLoadContextBuilder();

			builder.SetMainAssemblyPath(config.MainAssemblyPath);
			builder.SetDefaultContext(config.DefaultContext);

			foreach (var ext in config.PrivateAssemblies)
			{
				builder.PreferLoadContextAssembly(ext);
			}

			if (config.PreferSharedTypes)
			{
				builder.PreferDefaultLoadContext(true);
			}

			if (config.IsUnloadable || config.EnableHotReload)
			{
				builder.EnableUnloading();
			}

			if (config.LoadInMemory)
			{
				builder.PreloadAssembliesIntoMemory();
				builder.ShadowCopyNativeLibraries();
			}


			builder.IsLazyLoaded(config.IsLazyLoaded);
			foreach (var assemblyName in config.SharedAssemblies)
			{
				builder.PreferDefaultLoadContextAssembly(assemblyName);
			}


			// In .NET Core 3.0, this code is unnecessary because the API, AssemblyDependencyResolver, handles parsing these files.
			var baseDir = Path.GetDirectoryName(config.MainAssemblyPath);
			var assemblyFileName = Path.GetFileNameWithoutExtension(config.MainAssemblyPath);

			var depsJsonFile = Path.Combine(baseDir, assemblyFileName + ".deps.json");
			if (File.Exists(depsJsonFile))
			{
				builder.AddDependencyContext(depsJsonFile);
			}

			var pluginRuntimeConfigFile = Path.Combine(baseDir, assemblyFileName + ".runtimeconfig.json");

			builder.TryAddAdditionalProbingPathFromRuntimeConfig(pluginRuntimeConfigFile, includeDevConfig: true, out _);

			// Always include runtimeconfig.json from the host app.
			// in some cases, like `dotnet test`, the entry assembly does not actually match with the
			// runtime config file which is why we search for all files matching this extensions.
			foreach (var runtimeconfig in Directory.GetFiles(AppContext.BaseDirectory, "*.runtimeconfig.json"))
			{
				builder.TryAddAdditionalProbingPathFromRuntimeConfig(runtimeconfig, includeDevConfig: true, out _);
			}


			return builder;
		}
	}
}
