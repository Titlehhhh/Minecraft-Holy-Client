using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace HolyClient.ViewModels
{

	public class NavigationItemViewModel
	{
		[JsonProperty]
		private Guid id = Guid.NewGuid();
		public override bool Equals(object? obj)
		{

			if (obj is NavigationItemViewModel g)
			{
				return id == g.id;
			}
			return false;
		}

		[Reactive]
		public string Name { get; set; }
		[Reactive]
		public string Icon { get; set; }


		public IRoutableViewModel NavigationObject { get; set; }

		public NavigationItemViewModel()
		{

		}


	}
}
