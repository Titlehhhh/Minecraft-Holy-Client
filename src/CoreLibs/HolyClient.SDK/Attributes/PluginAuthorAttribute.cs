namespace HolyClient.SDK.Attributes;

public sealed class PluginAuthorAttribute : Attribute
{
    public PluginAuthorAttribute(string author)
    {
        Author = author;
    }

    public string Author { get; }
}