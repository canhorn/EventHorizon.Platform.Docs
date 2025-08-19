namespace Website.Shared.Components.NavTreeView.Model;

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
    public string Route { get; set; } = string.Empty;
    public IList<NavTreeViewNodeData> Children { get; set; } = [];

    public IEnumerable<NavTreeViewNodeData> Flatten()
    {
        var list = new List<NavTreeViewNodeData> { this };
        foreach (var child in Children)
        {
            list.AddRange(child.Flatten());
        }
        return list;
    }
}
