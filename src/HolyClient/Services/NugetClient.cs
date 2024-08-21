using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using HolyClient.Contracts.Models;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace HolyClient.Services;

public sealed class NugetClient : ReactiveObject, INugetClient
{
    private int _skip;
    private int _take = 20;

    private CancellationTokenSource CTS;

    public ISourceCache<IPackageSearchMetadata, string> Packages { get; } =
        new SourceCache<IPackageSearchMetadata, string>(x => x.Identity.Id);

    [Reactive] public string Filter { get; set; }

    [Reactive] public bool EnablePreviewVersions { get; set; }

    [Reactive] public bool IsLoading { get; private set; }

    [Reactive] public bool CanLoadNew { get; private set; }

    public async Task LoadNewItems(int count = 20)
    {
        _skip += count;
        _take += count;

        try
        {
            await DoSearch();
        }
        catch (Exception ex)
        {
        }
    }

    public async Task Refresh()
    {
        IsLoading = true;
        Packages.Clear();
        _skip = 0;
        _take = 20;
        try
        {
            await DoSearch();
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task DoSearch()
    {
        CTS?.Cancel();
        CTS = new CancellationTokenSource();

        var logger = NullLogger.Instance;
        var cancellationToken = CTS.Token;

        var repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
        var resource = await repository.GetResourceAsync<PackageSearchResource>();
        var searchFilter = new SearchFilter(EnablePreviewVersions);

        IEnumerable<IPackageSearchMetadata> results = await resource.SearchAsync(
            Filter,
            searchFilter,
            _skip,
            20,
            logger,
            cancellationToken);

        CanLoadNew = results.Count() == 20;


        Packages.AddOrUpdate(results);
    }
}