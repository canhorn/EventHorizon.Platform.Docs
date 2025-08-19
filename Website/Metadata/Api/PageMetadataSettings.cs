namespace Website.Metadata.Api;

using System.Collections.Generic;
using System.Reflection;

public class PageMetadataSettings
{
    public IEnumerable<Assembly> PageAssemblyList { get; }
    public IDictionary<string, float> FolderOrders { get; }

    public PageMetadataSettings(IEnumerable<Assembly> pageAssemblyList)
    {
        PageAssemblyList = pageAssemblyList;
        FolderOrders = GlobalPageConfig.PageOrders;
    }
}
