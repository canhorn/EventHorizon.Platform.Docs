namespace Website.Metadata.Api;

using System.Collections.Generic;
using System.Reflection;

public class PageMetadataSettings(string basePath, IEnumerable<Assembly> pageAssemblyList)
{
    public string BasePath { get; } = basePath;
    public IEnumerable<Assembly> PageAssemblyList { get; } = pageAssemblyList;
    public IDictionary<string, float> FolderOrders { get; } = GlobalPageConfig.PageOrders;
}
