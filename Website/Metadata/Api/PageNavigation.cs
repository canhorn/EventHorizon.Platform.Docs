namespace Website.Metadata.Api;

using System.Collections.Generic;

public interface PageNavigation
{
    public string Id { get; }
    public float Order { get; }
    public string Title { get; }
    public string Route { get; }
    public bool IsFolder { get; }
    public IEnumerable<PageNavigation> Children { get; }
    public IEnumerable<PageNavigation> Flatten();
}
