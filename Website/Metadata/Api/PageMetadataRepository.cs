namespace Website.Metadata.Api;

using System.Collections.Generic;
using Website.Metadata.Model;

public interface PageMetadataRepository
{
    public PageNavigation Nav();
    public IEnumerable<PageMetadataModel> All();
    public PageMetadataModel? Get(string route);

    public PageMetadataModel? NextPage(string currentRoute);
    public PageMetadataModel? PreviousPage(string currentRoute);
}
