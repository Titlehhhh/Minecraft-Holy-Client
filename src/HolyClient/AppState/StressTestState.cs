using DynamicData;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HolyClient.AppState;

[MessagePackObject(keyAsPropertyName:true)]
public sealed class StressTestState
{
	[IgnoreMember]
	public SourceCache<StressTestProfileState, Guid> Profiles { get; } = new(x => x.Id);

	public Guid SelectedProfileId { get; set; }

	public IEnumerable<StressTestProfileState> ProfilesStates
	{
		get => Profiles.Items.ToList();
		set => Profiles.AddOrUpdate(value);
	}
}
