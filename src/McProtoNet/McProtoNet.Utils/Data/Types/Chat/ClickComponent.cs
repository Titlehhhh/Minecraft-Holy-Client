using Newtonsoft.Json;

namespace McProtoNet.Utils
{
	public class ClickComponent
	{
		[JsonProperty(PropertyName = "action", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
		public EClickAction Action { get; set; }

		[JsonProperty(PropertyName = "value", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
		public string Value { get; set; }

		public string Translate { get; set; }

		public ClickComponent(EClickAction action, string value, string translate = "")
		{
			Action = action;
			Value = value;
			Translate = translate;
		}
	}
}
