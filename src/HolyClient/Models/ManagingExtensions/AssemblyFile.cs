using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Reflection;
using System.Threading.Tasks;
using HolyClient.Abstractions.StressTest;
using HolyClient.Core.Infrastructure;
using HolyClient.LoadPlugins;

namespace HolyClient.Models.ManagingExtensions;

public class AssemblyFile : IAssemblyFile
{
    private readonly Subject<Unit> _fileUpdate = new();


    private DateTime lastWriteTime = DateTime.MinValue;

    private FileSystemWatcher watcher;

    private readonly AssemblyWrapper wrapper;

    public AssemblyFile(string path)
    {
        FullPath = path;
        wrapper = new AssemblyWrapper(FullPath);
    }

    public string FullPath { get; }

    public string Name => Path.GetFileName(FullPath);
    public IObservable<Unit> FileUpdated => _fileUpdate;

    public Version Version { get; }


    public IEnumerable<Type> StressTestPlugins { get; private set; } = Enumerable.Empty<Type>();

    public async Task Initialization()
    {
        var directory = Path.GetDirectoryName(FullPath);


        await wrapper.Load();

        if (wrapper.CurrentState == PluginState.Loaded)
            StressTestPlugins = wrapper.CurrentAssembly
                .GetExportedTypes()
                .Where(x =>
                {
                    var g = !x.IsAbstract && typeof(IStressTestBehavior).IsAssignableFrom(x);
                    return g;
                })
                .ToArray();
        watcher = new FileSystemWatcher
        {
            Path = directory
        };

        watcher.NotifyFilter = NotifyFilters.LastWrite;


        watcher.Changed += OnChanged;
        watcher.Created += OnCreated;
        watcher.Deleted += OnDeleted;
        watcher.Renamed += OnRenamed;
        //watcher.Error += OnError;

        watcher.Filter = "*.dll";
        watcher.IncludeSubdirectories = true;
        watcher.EnableRaisingEvents = true;
    }

    public async Task UnLoad()
    {
        await wrapper.UnLoad();
        watcher.Changed -= OnChanged;
        watcher.Created -= OnCreated;
        watcher.Deleted -= OnDeleted;
        watcher.Renamed -= OnRenamed;
        watcher.Dispose();
    }

    private async void OnChanged(object sender, FileSystemEventArgs e)
    {
        try
        {
            await Task.Run(() =>
            {
                var name = AssemblyName.GetAssemblyName(FullPath);
            });
            _fileUpdate.OnNext(default);
        }
        catch (Exception ex)
        {
        }
    }

    private void OnCreated(object sender, FileSystemEventArgs e)
    {
    }

    private void OnDeleted(object sender, FileSystemEventArgs e)
    {
    }


    private void OnRenamed(object sender, RenamedEventArgs e)
    {
    }
}