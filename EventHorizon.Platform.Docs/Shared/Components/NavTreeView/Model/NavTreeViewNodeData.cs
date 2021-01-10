namespace EventHorizon.Platform.Docs.Shared.Components.NavTreeView.Model
{
    using System.Collections.Generic;

    public class NavTreeViewNodeData
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Href { get; set; } = string.Empty;
        public string IconCssClass { get; set; } = string.Empty;
        public bool IsExpanded { get; set; }
        public bool IsDisabled { get; set; }
        public IList<NavTreeViewNodeData> Children { get; set; } = new List<NavTreeViewNodeData>();
    }
}
