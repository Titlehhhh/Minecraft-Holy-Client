using DynamicData;
using HolyClient.StressTest;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HolyClient.AppState;

[MessagePackObject(keyAsPropertyName:true)]
public sealed class StressTestState
{
	[IgnoreMember]
	public SourceCache<IStressTestProfile, Guid> Profiles { get; } = new(x => x.Id);

	public Guid SelectedProfileId { get; set; }

	public IEnumerable<IStressTestProfile> ProfilesStates
	{
		get => Profiles.Items.ToList();
		set => Profiles.AddOrUpdate(value);
	}
}
