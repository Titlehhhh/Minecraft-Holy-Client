namespace HolyClient.SDK.Attributes;

public sealed class PluginDescriptionAttribute : Attribute
{
    public PluginDescriptionAttribute(string description)
    {
        Description = description;
    }

    public string Description { get; }
}