namespace HolyClient.SDK.Attributes;

public sealed class PluginTitleAttribute : Attribute
{
    public PluginTitleAttribute(string title)
    {
        Title = title;
    }

    public string Title { get; }
}