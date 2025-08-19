namespace Website.Metadata.Api;

using System.Collections.Generic;

public interface PageNavigation
{
    string Id { get; }
    float Order { get; }
    string Title { get; }
    string Route { get; }
    bool IsFolder { get; }
    IEnumerable<PageNavigation> Children { get; }
    IEnumerable<PageNavigation> Flatten();
}
