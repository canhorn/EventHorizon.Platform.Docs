namespace Website.Metadata.Api;

using Website.Metadata.Model;

public interface PageScopedState
{
    public delegate void OnCurrentPageChangedDelegate(string route);
    public event OnCurrentPageChangedDelegate OnCurrentPageChanged;
    public PageMetadataModel? CurrentPage { get; }
    public void SetCurrentPage(PageMetadataModel page);
}
