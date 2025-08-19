namespace Website.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Website.Localization;
using Website.Localization.Api;
using Website.Metadata.Api;
using Website.Shared.Components.NavTreeView.Model;

public partial class NavMenu : ComponentBase, IDisposable
{
    [Inject]
    public required Localizer<SharedResource> Localizer { get; set; }

    [Inject]
    public required PageMetadataRepository Repository { get; set; }

    [Inject]
    public required PageScopedState ScopeState { get; set; }

    private NavTreeViewNodeData NavigationTreeView { get; set; } = null!;
    private bool _collapseNavMenu = true;

    protected override void OnInitialized()
    {
        NavigationTreeView = BuildEditorTreeViewNode(null, Repository.Nav(), []);

        ScopeState.OnCurrentPageChanged += OnCurrentPageChanged;
    }

    private void ToggleNavMenu()
    {
        _collapseNavMenu = !_collapseNavMenu;
    }

    private void HandleNodeClicked(NavTreeViewNodeData node)
    {
        if (!node.Children?.Any() ?? true)
        {
            ToggleNavMenu();
        }
    }

    private NavTreeViewNodeData BuildEditorTreeViewNode(
        NavTreeViewNodeData? existingTreeView,
        PageNavigation model,
        IEnumerable<NavTreeViewNodeData> expandedList
    )
    {
        return new NavTreeViewNodeData
        {
            Id = model.Id,
            Name = model.Title,
            Text = Localizer[model.Title],
            Href = !model.IsFolder ? model.Route : string.Empty,
            IsDisabled = model.IsFolder && (model.Children == null || !model.Children.Any()),
            IconCssClass = string.Empty, //"--icon oi oi-" + (model.IsFolder ? "folder" : "file"),
            Children =
                model
                    .Children?.Select(childNode =>
                        BuildEditorTreeViewNode(existingTreeView, childNode, expandedList)
                    )
                    .ToList() ?? [],
            IsExpanded =
                GetExistingValueOrDefault(existingTreeView?.Children ?? [], model.Id)
                || GetExistingValueOrDefault(expandedList, model.Id),
        };
    }

    private static bool GetExistingValueOrDefault(
        IEnumerable<NavTreeViewNodeData> nodeChildren,
        string nodeDataId
    )
    {
        foreach (var nodeData in nodeChildren)
        {
            if (nodeData.Id == nodeDataId)
            {
                return nodeData.IsExpanded;
            }
            if (nodeData.Children != null && nodeData.Children.Count > 0)
            {
                var result = GetExistingValueOrDefault(nodeData.Children, nodeDataId);
                if (result)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void Dispose()
    {
        ScopeState.OnCurrentPageChanged -= OnCurrentPageChanged;
    }

    private void OnCurrentPageChanged(string route)
    {
        var node = NavigationTreeView.Flatten().FirstOrDefault(a => a.Route == route);
        if (node != null)
        {
            node.IsExpanded = true;
            var parent = NavigationTreeView
                .Flatten()
                .FirstOrDefault(a => a.Children.Any(b => b.Id == node.Id));
            if (parent != null)
            {
                parent.IsExpanded = true;
            }
        }
        StateHasChanged();
    }
}
