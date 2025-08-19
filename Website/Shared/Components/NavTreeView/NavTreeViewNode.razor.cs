namespace Website.Shared.Components.NavTreeView;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Website.Shared.Components.NavTreeView.Model;

public class NavTreeViewNodeModel : ComponentBase
{
    [Parameter]
    public NavTreeViewNodeData Node { get; set; } = null!;

    [Parameter]
    public EventCallback OnChanged { get; set; }

    [Parameter]
    public EventCallback<NavTreeViewNodeData> OnNodeClicked { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    protected bool IsParentNode => Node.Children?.Count > 0;

    protected string GetNodeHref()
    {
        return !string.IsNullOrWhiteSpace(Node.Href) ? Node.Href : "javascript:;";
    }

    protected string GetIconClass()
    {
        return Node.IconCssClass;
    }

    protected string GetExpandedIconClass()
    {
        if (Node.IsExpanded)
        {
            return "oi oi-minus"; // TODO: Move this to [Parameter]
        }
        return "oi oi-plus"; // TODO: Move this to [Parameter]
    }

    protected string GetAriaExpanded()
    {
        return Node.IsExpanded.ToLower();
    }

    protected async Task HandleClickOfNode()
    {
        await OnNodeClicked.InvokeAsync(Node);
        if (Node.IsDisabled || !IsParentNode)
        {
            return;
        }
        Node.IsExpanded = !Node.IsExpanded;
        await OnChanged.InvokeAsync();
    }

    protected string GetNavLinkClass()
    {
        return $"tree-view__node-link --clickable";
    }
}
