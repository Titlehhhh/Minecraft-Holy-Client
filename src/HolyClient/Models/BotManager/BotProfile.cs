using HolyClient.Common;
using HolyClient.Contracts.Models;
using HolyClient.Contracts.Services;
using HolyClient.Core.Models.BotManager;
using McProtoNet;
using MessagePack;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace HolyClient.Models;
[MessagePackObject(keyAsPropertyName: true)]
public sealed class BotProfile : ReactiveObject, IBotProfile
{

	[Reactive]
	public Guid Id { get; set; }
	[Reactive]
	public string Name { get; set; }
	[Reactive]
	public string Server { get; set; }
	[Reactive]
	public string Nickname { get; set; }
	[Reactive]
	public MinecraftVersion Version { get; set; } = MinecraftVersion.MC_1_16_5_Version;

	[Reactive]
	public bool IsAuth { get; set; }

	[Reactive]
	public AuthInfo Auth { get; set; } = new();

	[Reactive]
	public bool ProxyUsed { get; set; }
	[Reactive]
	public ProxyInfo Proxy { get; set; } = new();
	[Reactive]
	public int SelectedTab { get; set; }

	private List<BotPluginReference> _plugins = new();

	public IEnumerable<BotPluginReference> PluginReferences
	{
		get
		{
			return _plugins;
		}
		set
		{
			_plugins.AddRange(value);
		}
	}
	[IgnoreMember]
	public IBotPluginStore PluignStore { get; }
	[Reactive]
	public BotState CurrentState { get; private set; }


	private MinecraftBot bot = new MinecraftBot();

	private volatile IDisposable? _cleanUp;
	public BotProfile()
	{
		CompositeDisposable d = new CompositeDisposable();
		this.PluignStore = new BotPluginStore(bot, this._plugins);

		bot.StateObservable
			.Subscribe(x => this.CurrentState = x)
			.DisposeWith(d);

		d.Add(bot);

		_cleanUp = d;
	}






	public void Dispose()
	{
		Interlocked.Exchange(ref _cleanUp, null)?.Dispose();
	}

	public void Start(Serilog.ILogger logger)
	{
		bot.Nickname = this.Nickname;

		string[] hostPort = this.Server.Split(':');
		ushort port = 25565;

		string host = hostPort[0];

		if (hostPort.Length == 2)
		{
			port = ushort.Parse(hostPort[1]);
		}

		bot.Host = host;
		bot.Port = port;

		bot.Version = this.Version;

		_ = bot.Run(logger, default);

	}

	public void Stop()
	{
		bot.Stop();
	}

	//public Task Initialization(IPluginProvider provider)
	//{
	//	return this.PluignStore.Initialization(provider, this.PluginReferences);
	//}
}



