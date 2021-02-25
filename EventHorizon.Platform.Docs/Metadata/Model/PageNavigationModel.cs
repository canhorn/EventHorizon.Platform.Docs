namespace EventHorizon.Platform.Docs.Metadata.Model
{
    using System.Collections.Generic;
    using EventHorizon.Platform.Docs.Metadata.Api;

    public class PageNavigationModel
        : PageNavigation
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public bool IsFolder { get; set; }
        public IEnumerable<PageNavigation> Children => ChildrenAsList;
        public List<PageNavigationModel> ChildrenAsList { get; set; } = new List<PageNavigationModel>();
    }
}
