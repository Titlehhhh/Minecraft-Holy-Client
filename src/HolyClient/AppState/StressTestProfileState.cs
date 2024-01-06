using McProtoNet;
using MessagePack;
using System;
using System.Collections.Generic;

namespace HolyClient.AppState;

[MessagePackObject(keyAsPropertyName: true)]
public sealed class StressTestProfileState
{
	

	public Guid Id { get; set; } = Guid.NewGuid();

	#region General
	
	public string BotsNicknames { get; set; }
	public int NumberOfBots { get; set; }
	public bool UseProxy { get; set; }
	public MinecraftVersion Version { get; set; }
	#endregion

	#region Proxies

	public IEnumerable<ProxySourceState> Proxies { get; set; }

	#endregion

	#region Behavior

	public BehaviorKey CurrentBehavior { get; set; }

	#endregion



}
