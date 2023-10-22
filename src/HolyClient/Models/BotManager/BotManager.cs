using DynamicData;
using HolyClient.Contracts.Models;
using HolyClient.Contracts.Services;
using MessagePack;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyClient.Models;

[MessagePackObject(keyAsPropertyName: true)]

public class BotManager : ReactiveObject, IBotManager
{
	[IgnoreMember]
	private SourceCache<IBotProfile, Guid> _profiles = new(x => x.Id);

	[IgnoreMember]
	public IConnectableCache<IBotProfile, Guid> Profiles => _profiles;


	[Key(0)]
	public IEnumerable<IBotProfile> ProfilesStates
	{
		get
		{
			return _profiles.Items.ToList();
		}
		set
		{
			_profiles.AddOrUpdate(value);
		}
	}

	[Reactive]
	[Key(1)]
	public Guid SelectedProfile { get; set; }

	public IBotProfile CreateAndAddBot()
	{

		var newProfile = new BotProfile();
		newProfile.Id = Guid.NewGuid();
		_profiles.AddOrUpdate(newProfile);

		return newProfile;
	}

	public async Task Initialization()
	{
		var provider = Locator.Current.GetService<IPluginProvider>();

		foreach (var profile in this.ProfilesStates)
		{
			await profile.Initialization(provider);
		}
	}

	public void RemoveBot(Guid id)
	{
		var profile = _profiles.Lookup(id);


		_profiles.RemoveKey(id);
	}
}

