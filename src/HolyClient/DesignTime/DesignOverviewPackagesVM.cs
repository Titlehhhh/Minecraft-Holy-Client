using System.Collections.ObjectModel;
using HolyClient.Services;
using HolyClient.ViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace HolyClient.DesignTime;

public class DesignOverviewPackagesVM : ReactiveObject
{
    private NugetClient nugetClient = new();

    public DesignOverviewPackagesVM()
    {
        NugetPackageViewModel test = new()
        {
            Id = "McProtoNet",
            Authors = "Title",
            DownloadCount = 1000000,
            Icon = null,
            Description = "Лучшая бибилотека для c#"
            //LicenseUrl = "https://licenses.nuget.org/MIT",
            //ProjectUrl = "https://www.newtonsoft.com/json",
            //Versions = new string[]
            // {
            //	 "7.0.0",
            //	 "6.0.0",
            //	 "5.0.0",
            //	 "4.0.0",
            //	 "3.0.0",
            //	 "2.0.0",
            //	 "1.0.0"
            // },
            //SelectedVesrion = "7.0.0",
            //Published = DateTimeOffset.Now.Date
        };
        Packages.Add(test);
        SelectedPackage = test;
    }

    public ObservableCollection<NugetPackageViewModel> Packages { get; } = new();

    public string Filter { get; set; }

    [Reactive] public NugetPackageViewModel? SelectedPackage { get; set; }

    public bool IsLoadingNewItems => false;

    public bool IsLoading => false;
}