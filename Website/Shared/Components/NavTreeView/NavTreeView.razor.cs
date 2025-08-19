namespace Website.Shared.Components.NavTreeView;

using Microsoft.AspNetCore.Components;
using Website.Shared.Components.NavTreeView.Model;

public class NavTreeViewModel : ComponentBase
{
    [Parameter]
    public NavTreeViewNodeData Root { get; set; } = null!;

    [Parameter]
    public EventCallback OnChanged { get; set; }

    [Parameter]
    public EventCallback<NavTreeViewNodeData> OnNodeClicked { get; set; }
}
