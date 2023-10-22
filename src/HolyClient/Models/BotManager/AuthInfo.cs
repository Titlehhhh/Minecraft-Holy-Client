using McProtoNet.Utils;
using MessagePack;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace HolyClient.Models;

[MessagePackObject(keyAsPropertyName: true)]
public sealed class AuthInfo : ReactiveObject
{

	[Reactive]
	public AccountType Type { get; set; }


	[Reactive]
	public string Password { get; set; }


}

