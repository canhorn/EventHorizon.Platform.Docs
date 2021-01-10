namespace EventHorizon.Platform.Docs.Metadata.Api
{
    using System.Collections.Generic;

    public interface PageNavigation
    {
        string Id { get; }
        string Title { get; }
        string Route { get; }
        bool IsFolder { get; }
        IEnumerable<PageNavigation> Children { get; }
    }
}
