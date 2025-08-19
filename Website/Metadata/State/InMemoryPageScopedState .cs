namespace Website.Metadata.State;

using Website.Metadata.Api;
using Website.Metadata.Model;

public class InMemoryPageScopedState : PageScopedState
{
    public PageMetadataModel? CurrentPage { get; private set; }

    public event PageScopedState.OnCurrentPageChangedDelegate OnCurrentPageChanged = delegate { };

    public void SetCurrentPage(PageMetadataModel page)
    {
        CurrentPage = page;
        OnCurrentPageChanged(page.Route);
    }
}
