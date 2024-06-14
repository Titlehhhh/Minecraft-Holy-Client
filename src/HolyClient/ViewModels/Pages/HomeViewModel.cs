using System;
using System.Threading.Tasks;
using HolyClient.Localization;
using ReactiveUI;

namespace HolyClient.ViewModels;

public class HomeViewModel : ReactiveObject, IActivatableViewModel, IRoutableViewModel, IHomeViewModel
{
    public HomeViewModel()
    {
        Loc.Instance.CurrentLanguageChanged += Instance_CurrentLanguageChanged;
        this.WhenActivated(Activated);
    }

    public ViewModelActivator Activator { get; } = new();

    public string Description { get; }

    public string ReleaseNotes { get; private set; }

    public string Version
    {
        get
        {
            if (ApplicationDeployment.CurrentDeployment is not null)
                return ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            return "Not deploy";
        }
    }

    public string? UrlPathSegment => "/home";

    public IScreen HostScreen { get; }

    private void Instance_CurrentLanguageChanged(object? sender, CurrentLanguageChangedEventArgs e)
    {
        DownloadReleaseNotes();
    }

    private void Activated(IDisposable d)
    {
        DownloadReleaseNotes();
    }

    private async void DownloadReleaseNotes()
    {
        await Task.Run(async () =>
        {
            try
            {
                //string source = "https://raw.githubusercontent.com/Titlehhhh/Minecraft-Holy-Client/master/ReleaseNotes/";
                //Version version = ApplicationDeployment.CurrentDeployment.CurrentVersion;
                //string ver = version.ToString().Replace(".", "_");

                //string lang = Loc.Instance.CurrentLanguage;
                //string readmePath = "Readme.md";
                //if (lang != "ru")
                //{
                //	readmePath = "Readme." + lang + ".md";
                //}

                //source = $"{source}{ver}/" + readmePath;

                //using (HttpClient client = new HttpClient())
                //{
                //	string markdown = await client.GetStringAsync(source);
                //	ReleaseNotes = markdown;
                //}
            }
            catch (Exception e)
            {
                ReleaseNotes = "Error: " + e.Message;
            }
        });
    }
}