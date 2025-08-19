namespace Website.Metadata.Model;

using System.Reflection;
using Microsoft.AspNetCore.Components;
using Website.Localization;
using Website.Localization.Api;
using Website.Metadata.Api;

public class PageMetadataBase : ComponentBase, PageMetadata
{
    [Inject]
    public required Localizer<SharedResource> Localizer { get; set; }

    [Inject]
    public required PageMetadataRepository Repository { get; set; }

    [Inject]
    public required PageScopedState ScopedState { get; set; }

    public PageMetadataModel PageMetadata { get; private set; } = new PageMetadataModel();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        var route = GetType().GetCustomAttribute<RouteAttribute>()?.Template ?? "";
        if (string.IsNullOrEmpty(route))
        {
            return;
        }
        var page = Repository.Get(route);
        if (page is null)
        {
            return;
        }

        PageMetadata = page;
        ScopedState.SetCurrentPage(page);
    }
}
