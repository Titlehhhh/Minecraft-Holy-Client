﻿using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading;
using System.Windows.Input;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace HolyClient.ViewModels;

public sealed class NugetPackageViewModel : ReactiveObject, IActivatableViewModel
{
    private NuGetVersion? _lastVersion;

    public NugetPackageViewModel(IPackageSearchMetadata model)
    {
        this.model = model;
        Icon = model.IconUrl;
        Id = model.Identity.Id;
        Description = model.Description;
        DownloadCount = model.DownloadCount;
        Authors = model.Authors;

        this.WhenActivated(d =>
        {
            LoadVersionsAndDownlaodCount();


            this.WhenAnyValue(x => x.SelectedVersion)
                .Subscribe(OnChangeSelectedVersion)
                .DisposeWith(d);
        });
    }

    public NugetPackageViewModel()
    {
    }


    [Reactive] public bool IsLoadingVersions { get; set; } = true;

    [Reactive] public bool IsLoadingMetadata { get; set; } = true;

    [Reactive] public NugetMetadataViewModel Metadata { get; set; }

    public ICommand InstallCommand { get; }

    public ViewModelActivator Activator { get; } = new();

    private async void OnChangeSelectedVersion(NuGetVersion version)
    {
        IsLoadingMetadata = true;
        try
        {
            var logger = NullLogger.Instance;
            var cancellationToken = CancellationToken.None;

            var cache = new SourceCacheContext();
            var repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
            var resource = await repository.GetResourceAsync<PackageMetadataResource>();

            var metadata =
                await resource.GetMetadataAsync(new PackageIdentity(Id, version), cache, logger, cancellationToken);

            var vm = new NugetMetadataViewModel(metadata);

            Metadata = vm;
        }
        catch
        {
        }
        finally
        {
            IsLoadingMetadata = false;
        }
    }

    private async void LoadVersionsAndDownlaodCount()
    {
        IsLoadingVersions = true;
        try
        {
            var versions = await model.GetVersionsAsync();

            if (DownloadCount is null)
                DownloadCount = versions.Select(x => x.DownloadCount).FirstOrDefault(x => x is not null);

            var versions2 = versions.OrderByDescending(x => x.Version)
                .Select(x => x.Version);
            _lastVersion = versions2.FirstOrDefault();
            Versions = versions2.ToArray();
            SelectedVersion = _lastVersion;
        }
        catch
        {
        }
        finally
        {
            IsLoadingVersions = false;
        }
    }


    #region Nuget Properties

    public Uri? Icon { get; set; }
    public string Authors { get; set; }

    public string Description { get; set; }

    [Reactive] public long? DownloadCount { get; set; }

    public string Id { get; set; }

    public string Owners { get; set; }

    public bool PrefixReserved { get; set; }

    [Reactive] public NuGetVersion? SelectedVersion { get; set; }

    [Reactive] public NuGetVersion[] Versions { get; set; }

    private readonly IPackageSearchMetadata model;

    #endregion
}

public class NugetMetadataViewModel
{
    public NugetMetadataViewModel(IPackageSearchMetadata metadata)
    {
        Description = metadata.Description;
        Authors = metadata.Authors;
        DownloadCount = metadata.DownloadCount;
        Published = metadata.Published;
        Version = metadata.Identity.Version.Version.ToString();
    }

    public string Description { get; }

    public string Authors { get; }
    public long? DownloadCount { get; }
    public DateTimeOffset? Published { get; }

    public string Version { get; }
}