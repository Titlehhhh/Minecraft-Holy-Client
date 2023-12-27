using HolyClient.StressTest;
using QuickProxyNet;
using ReactiveUI;
using System;

namespace HolyClient.ViewModels;

public class ProxySourceViewModel : ReactiveObject
{
	public Guid Id { get; private set; }
	//public int AveragePing => Random.Shared.Next(100, 500);

	public string Name { get; set; }

	public string Icon { get; private set; }

	public ProxyType Type { get; set; }
	public ProxySourceViewModel(IProxySource proxySource)
	{
		Id = proxySource.Id;

		Name = proxySource.Name;

		Type = proxySource.Type;

		Icon = proxySource switch
		{
			UrlProxySource => "UrlProxy",
			FileProxySource => "FileProxy",
			InMemoryProxySource => "InMemoryProxy"
		};

	}
}