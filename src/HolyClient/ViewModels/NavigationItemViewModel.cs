using System;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace HolyClient.ViewModels;

public class NavigationItemViewModel
{
    [JsonProperty] private Guid id = Guid.NewGuid();

    [Reactive] public string Name { get; set; }

    [Reactive] public string Icon { get; set; }


    public IRoutableViewModel NavigationObject { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is NavigationItemViewModel g) return id == g.id;
        return false;
    }
}