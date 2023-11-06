namespace HolyClient.SDK.Attributes
{
	public sealed class PluginAuthorAttribute : Attribute
	{
		public string Author { get; }

		public PluginAuthorAttribute(string author)
		{
			Author = author;
		}
	}
}
