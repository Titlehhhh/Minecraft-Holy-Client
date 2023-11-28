namespace HolyClient.SDK.Attributes
{
	public sealed class PluginDescriptionAttribute : Attribute
	{
		public string Description { get; }

		public PluginDescriptionAttribute(string description)
		{
			Description = description;
		}
	}
}
