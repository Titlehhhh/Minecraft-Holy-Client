namespace HolyClient.SDK.Attributes
{
	public sealed class PluginTitleAttribute : Attribute
	{
		public string Title { get; }

		public PluginTitleAttribute(string title)
		{
			Title = title;
		}
	}
}
