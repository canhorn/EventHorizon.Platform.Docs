namespace Website.Metadata.Model;

public class PageMetadataModel
{
    public float Order { get; set; }
    public string Route { get; set; } = string.Empty;
    public bool InNavigation { get; set; }
    public string Title { get; set; } = string.Empty;
}
