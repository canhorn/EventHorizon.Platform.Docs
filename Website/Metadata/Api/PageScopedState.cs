namespace Website.Metadata.Api;

using Website.Metadata.Model;

public interface PageScopedState
{
    delegate void OnCurrentPageChangedDelegate(string route);
    event OnCurrentPageChangedDelegate OnCurrentPageChanged;
    PageMetadataModel? CurrentPage { get; }
    void SetCurrentPage(PageMetadataModel page);
}
