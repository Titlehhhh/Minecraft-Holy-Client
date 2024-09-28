using System;
using HolyClient.StressTest;
using QuickProxyNet;
using ReactiveUI;

namespace HolyClient.ViewModels;

public class ProxySourceViewModel : ReactiveObject
{
    public ProxySourceViewModel(IProxySource proxySource)
    {
        Id = proxySource.Id;

        Name = proxySource.Name;

        string type = "None";
        if (proxySource.Type is not null)
        {
            Type = proxySource.Type.ToString();
        }
        Type = type;
        

        Icon = proxySource switch
        {
            UrlProxySource => "UrlProxy",
            FileProxySource => "FileProxy",
            InMemoryProxySource => "InMemoryProxy"
        };
    }

    public Guid Id { get; private set; }
    //public int AveragePing => Random.Shared.Next(100, 500);

    public string Name { get; set; }

    public string Icon { get; private set; }

    public string Type { get; set; }
}