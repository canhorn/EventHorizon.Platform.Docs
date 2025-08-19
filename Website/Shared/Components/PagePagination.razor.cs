namespace Website.Shared.Components;

using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Website.Localization;
using Website.Localization.Api;
using Website.Metadata.Api;
using Website.Metadata.Model;

public class PagePaginationBase : ComponentBase, IDisposable
{
    [Inject]
    public required Localizer<SharedResource> Localizer { get; set; }

    [Inject]
    public required PageMetadataRepository Repository { get; set; }

    [Inject]
    public required PageScopedState ScopedState { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }
    public string CurrentPageUrl { get; private set; } = "";
    public PageMetadataModel? CurrentPage { get; private set; }
    public PageMetadataModel? NextPage { get; private set; }
    public PageMetadataModel? PreviousPage { get; private set; }

    public void Dispose()
    {
        ScopedState.OnCurrentPageChanged -= OnCurrentPageChanged;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        ScopedState.OnCurrentPageChanged += OnCurrentPageChanged;
        SetCurrentPage();
        CurrentPageUrl = NavigationManager.Uri.Replace(NavigationManager.BaseUri, "/");
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        CurrentPageUrl = NavigationManager.Uri.Replace(NavigationManager.BaseUri, "/");
        SetCurrentPage();
        StateHasChanged();
    }

    private void SetCurrentPage()
    {
        NextPage = null;
        PreviousPage = null;
        CurrentPage = null;
        if (ScopedState.CurrentPage?.Route != CurrentPageUrl)
        {
            return;
        }
        CurrentPage = ScopedState.CurrentPage;
        if (CurrentPage is not null)
        {
            NextPage = Repository.NextPage(CurrentPage.Route);
            PreviousPage = Repository.PreviousPage(CurrentPage.Route);
        }
    }

    private void OnCurrentPageChanged(string route)
    {
        SetCurrentPage();
        StateHasChanged();
    }
}
