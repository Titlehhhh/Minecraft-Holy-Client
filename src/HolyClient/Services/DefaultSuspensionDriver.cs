using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using HolyClient.StressTest;
using MessagePack;
using MessagePack.Resolvers;
using ReactiveUI;

namespace HolyClient.Services;

public class DefaultSuspensionDriver<TAppState> : ISuspensionDriver
    where TAppState : class
{
    private readonly string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

    private readonly string dataFileName = "appstate.dat";
    private MessagePackSerializerOptions options;
    private readonly IFormatterResolver resolver;
    private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

    public DefaultSuspensionDriver()
    {
        resolver = CompositeResolver.Create(
            CompositeResolver.Create(PluginTypeRefFormatter.Instance),
            BuiltinResolver.Instance,
            AttributeFormatterResolver.Instance,

            // replace enum resolver
            DynamicEnumAsStringResolver.Instance,
            DynamicGenericResolver.Instance,
            DynamicUnionResolver.Instance,
            DynamicObjectResolver.Instance,
            PrimitiveObjectResolver.Instance,

            // final fallback(last priority)
            DynamicContractlessObjectResolver.Instance
        );
        options = MessagePackSerializerOptions.Standard.WithResolver(resolver);
    }

    public IObservable<Unit> InvalidateState()
    {
        return Observable.Empty<Unit>();
    }

    public IObservable<object> LoadState()
    {
        return Observable.FromAsync(async () =>
        {
            await _lock.WaitAsync().ConfigureAwait(false);
            try
            {
                var path = Path.Combine(appData, App.AppName);
                if (Directory.Exists(path))
                {
                    path = Path.Combine(path, dataFileName);

                    if (File.Exists(path))
                    {
                        var fs = File.OpenRead(path);
                        await using (fs.ConfigureAwait(false))
                        {
                            TAppState state = await MessagePackSerializer.DeserializeAsync<TAppState>(fs)
                                .ConfigureAwait(false);
                            return state;
                        }
                    }

                    throw new FileNotFoundException($"Не найден файл с состоянием на пути: {path}");
                }

                throw new DirectoryNotFoundException($"Не найдена папка приложения: {path}");
            }
            finally
            {
                _lock.Release();
            }
        });
    }

    public IObservable<Unit> SaveState(object state)
    {
        return Observable.FromAsync<Unit>(async () =>
        {
            await _lock.WaitAsync().ConfigureAwait(false);
            try
            {
                var path = Path.Combine(appData, App.AppName);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                path = Path.Combine(path, dataFileName);

                var fs = File.OpenWrite(path);
                await using (fs.ConfigureAwait(false))
                {
                    await MessagePackSerializer.SerializeAsync(fs, state).ConfigureAwait(false);
                }

                return Unit.Default;
            }
            finally
            {
                _lock.Release();
            }
        });
    }
}