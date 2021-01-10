namespace EventHorizon.Platform.Docs.Shared.Components.NavTreeView
{
    using EventHorizon.Platform.Docs.Shared.Components.NavTreeView.Model;
    using Microsoft.AspNetCore.Components;

    public class NavTreeViewModel
        : ComponentBase
    {
        [Parameter]
        public NavTreeViewNodeData Root { get; set; } = null!;
        [Parameter]
        public EventCallback OnChanged { get; set; }
        [Parameter]
        public EventCallback<NavTreeViewNodeData> OnNodeClicked { get; set; }
    }
}
