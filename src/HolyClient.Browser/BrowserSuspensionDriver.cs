using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.InteropServices.JavaScript;

public sealed partial class BrowserSuspensionDriver : ISuspensionDriver
{
	private readonly JsonSerializerSettings _settings = new()
	{
		TypeNameHandling = TypeNameHandling.All,
		NullValueHandling = NullValueHandling.Ignore,
		ObjectCreationHandling = ObjectCreationHandling.Replace,
		ContractResolver = new CamelCasePropertyNamesContractResolver(),
	};

	private static string Key = "HolyClient.AppState";

	[JSImport("globalThis.localStorage.setItem")]
	private static partial void SetItem(string key, string value);

	[JSImport("globalThis.localStorage.getItem")]
	private static partial string GetItem(string key);

	public IObservable<Unit> InvalidateState()
	{
		return Observable.Return(Unit.Default);
	}

	public IObservable<object> LoadState()
	{
		var lines = GetItem(Key);
		var state = JsonConvert.DeserializeObject<object>(lines, _settings);
		return Observable.Return(state);
	}

	public IObservable<Unit> SaveState(object state)
	{
		var lines = JsonConvert.SerializeObject(state, _settings);
		SetItem(Key, lines);
		return Observable.Return(Unit.Default);
	}
}