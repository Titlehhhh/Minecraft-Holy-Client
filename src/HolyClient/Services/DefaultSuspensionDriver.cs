using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
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
        try
        {
            var path = Path.Combine(appData, App.AppName);
            if (Directory.Exists(path))
            {
                path = Path.Combine(path, dataFileName);

                if (File.Exists(path))
                {
                    var data = File.ReadAllBytes(path);

                    var state = MessagePackSerializer.Deserialize<TAppState>(data);
                    if (state is null) throw new NullReferenceException("App State is null");

                    return Observable.Return(state);
                }

                throw new FileNotFoundException($"Не найден файл с состоянием на пути: {path}");
            }

            throw new DirectoryNotFoundException($"Не найдена папка приложения: {path}");
        }
        catch (Exception ex)
        {
            return Observable.Throw<object>(ex);
        }
    }

    public IObservable<Unit> SaveState(object state)
    {
        var path = Path.Combine(appData, App.AppName);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        path = Path.Combine(path, dataFileName);


        var data = MessagePackSerializer.Serialize((TAppState)state);

        using (var sw = new StreamWriter(path))
        {
            sw.BaseStream.Write(data);
        }

        return Observable.Return(Unit.Default);
    }
}