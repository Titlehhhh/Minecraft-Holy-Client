namespace HolyClient.Core.Infrastructure;

public sealed class PluginMetadata
{
    public PluginMetadata(string? author, string? description, string? title)
    {
        Author = author;
        Description = description;
        Title = title;
    }

    public string? Author { get; private set; }
    public string? Description { get; private set; }
    public string? Title { get; private set; }
}