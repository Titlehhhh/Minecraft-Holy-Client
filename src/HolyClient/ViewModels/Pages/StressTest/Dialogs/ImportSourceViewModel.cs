namespace HolyClient.ViewModels.Pages.StressTest.Dialogs;

public class ImportSourceViewModel
{
    public ImportSourceViewModel(string icon, ImportSource sourceType)
    {
        SourceType = sourceType;
        Icon = icon;
        var baseTr = $"StressTest.Configuration.Proxy.Dialog.SelectSource.{icon}";
        Description = $"{baseTr}.Description";
        Name = baseTr;
    }

    public string Icon { get; }
    public string Description { get; }
    public string Name { get; }
    public ImportSource SourceType { get; }
}