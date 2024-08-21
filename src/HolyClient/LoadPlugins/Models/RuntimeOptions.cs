namespace HolyClient.LoadPlugins.Models;

internal class RuntimeOptions
{
    public string? Tfm { get; set; }

    public string[]? AdditionalProbingPaths { get; set; }
}