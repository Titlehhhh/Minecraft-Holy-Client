using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using HolyClient.Core.Infrastructure;
using HolyClient.StressTest;
using MessagePack;

namespace HolyClient.AppState;

[MessagePackObject(true)]
public sealed class StressTestState
{
    [IgnoreMember] public SourceCache<IStressTestProfile, Guid> Profiles { get; } = new(x => x.Id);

    public Guid SelectedProfileId { get; set; }

    public IEnumerable<IStressTestProfile> ProfilesStates
    {
        get => Profiles.Items.ToList();
        set => Profiles.AddOrUpdate(value);
    }

    internal async Task Initialization(IPluginProvider? pluginProvider)
    {
        foreach (var p in ProfilesStates) await p.Initialization(pluginProvider);
    }
}