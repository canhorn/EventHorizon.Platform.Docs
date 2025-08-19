namespace Website.Metadata.State;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Website.Metadata.Api;
using Website.Metadata.Attributes;
using Website.Metadata.Model;

public class StandardPageMetadataRepository : PageMetadataRepository
{
    private readonly ConcurrentDictionary<string, PageMetadataModel> _map;
    private readonly PageNavigation _nav;

    public StandardPageMetadataRepository(PageMetadataSettings settings)
    {
        _map = GenerateListOfPageMetadata(settings);
        _nav = BuildPageNavigation(
            _map.Where(a => a.Value.InNavigation).ToDictionary(),
            settings.FolderOrders
        );
    }

    public IEnumerable<PageMetadataModel> All()
    {
        return _map.Values;
    }

    public PageMetadataModel? Get(string route)
    {
        return _map.TryGetValue(route, out var page) ? page : null;
    }

    public PageMetadataModel? NextPage(string currentRoute)
    {
        var current = _map[currentRoute];
        var flattenedNav = _nav.Flatten().SkipWhile(a => a.Route != currentRoute).Skip(1).ToList();
        var page = flattenedNav.FirstOrDefault();
        if (page?.IsFolder ?? false)
        {
            page = flattenedNav.FirstOrDefault(a => !a.IsFolder);
        }
        if (page == null || string.IsNullOrEmpty(page.Route))
        {
            return null;
        }
        return _map[page.Route];
    }

    public PageMetadataModel? PreviousPage(string currentRoute)
    {
        var current = _map[currentRoute];
        var flattenedNav = _nav.Flatten().TakeWhile(a => a.Route != currentRoute).ToList();
        var page = flattenedNav.LastOrDefault();
        if (page?.IsFolder ?? false)
        {
            page = flattenedNav.LastOrDefault(a => !a.IsFolder);
        }
        if (page == null || string.IsNullOrEmpty(page.Route))
        {
            return null;
        }
        return _map[page.Route];
    }

    public PageNavigation Nav()
    {
        return _nav;
    }

    private static ConcurrentDictionary<string, PageMetadataModel> GenerateListOfPageMetadata(
        PageMetadataSettings settings
    )
    {
        var pageList = new Dictionary<string, PageMetadataModel>();
        var pageFileNameList = new List<string>();
        // Get All Pages
        var pageMetadataList = settings
            .PageAssemblyList.SelectMany(x => x.DefinedTypes)
            .Where(type => typeof(PageMetadata).IsAssignableFrom(type));

        foreach (var typeInfo in pageMetadataList)
        {
            if (Attribute.IsDefined(typeInfo, typeof(PageAttribute), false))
            {
                var pageAttribute = typeInfo.GetCustomAttribute<PageAttribute>();
                var routeAttribute = typeInfo.GetCustomAttribute<RouteAttribute>();
                pageFileNameList.Add(typeInfo.Name);
                if (routeAttribute == null)
                {
                    throw new SystemException(
                        $"Page Metadata needs a RouteAttribute to function: {typeInfo.Name}"
                    );
                }
                var model = new PageMetadataModel { Title = typeInfo.Name };
                // If has the PageMetadata Attribute pull data from that first
                if (Attribute.IsDefined(typeInfo, typeof(PageMetadataAttribute)))
                {
                    var pageMetadataAttribute =
                        typeInfo.GetCustomAttribute<PageMetadataAttribute>();
                    model.Title = string.IsNullOrEmpty(pageMetadataAttribute?.Title)
                        ? typeInfo.Name
                        : pageMetadataAttribute.Title;
                    model.Order = pageMetadataAttribute?.Order ?? 0;
                    model.InNavigation = pageMetadataAttribute?.InNavigation ?? true;
                }

                model.Route = routeAttribute.Template;

                pageList.Add(routeAttribute.Template, model);
            }
        }

#pragma warning disable IDE0306 // Simplify collection initialization
        return new ConcurrentDictionary<string, PageMetadataModel>(
            pageList.OrderBy(a => a.Value.Order)
        );
#pragma warning restore IDE0306 // Simplify collection initialization
    }

    private static PageNavigationModel BuildPageNavigation(
        IDictionary<string, PageMetadataModel> pageList,
        IDictionary<string, float> folderOrders
    )
    {
        var pathList = pageList.Keys;
        var root = new PageNavigationModel
        {
            Id = "root",
            Title = "root",
            IsFolder = true,
            ChildrenAsList = [],
        };

        foreach (var path in pathList)
        {
            AddNavigationModelToNode(pageList, root, path, folderOrders);
        }
        root.ChildrenAsList =
            root.ChildrenAsList?.OrderBy(a => a.Order)
                ?.ThenBy(a => a.Title)
                ?.ThenBy(a => a.ChildrenAsList != null)
                ?.ToList() ?? [];

        return root;
    }

    private static void AddNavigationModelToNode(
        IDictionary<string, PageMetadataModel> pageList,
        PageNavigationModel root,
        string path,
        IDictionary<string, float> folderOrders
    )
    {
        var splitPath = path.Split("/", StringSplitOptions.RemoveEmptyEntries).ToList();
        var node = splitPath.Last();
        var parentList = splitPath.Take(splitPath.Count - 1);
        var parent = root;

        foreach (var newParentPath in parentList)
        {
            var newParent = default(PageNavigationModel);

            if (
                parent.Children.Any(a => a.Id == newParentPath)
                && parent.Children.First(a => a.Id == newParentPath).IsFolder
            )
            {
                // Set newParent to already existing
                newParent = (PageNavigationModel)parent.Children.First(a => a.Id == newParentPath);
            }
            else if (
                parent.Children.Any(a => a.Id == newParentPath)
                && !parent.Children.First(a => a.Id == newParentPath).IsFolder
            )
            {
                // Update Parent Node to Parent Folder
                // And add Node to Folder
                newParent = (PageNavigationModel)parent.Children.First(a => a.Id == newParentPath);
                var newParentAsNode = new PageNavigationModel
                {
                    Id = newParent.Id,
                    Order = newParent.Order,
                    Title = newParent.Title,
                    Route = newParent.Route,
                };
                // Set Parent to Folder
                newParent.IsFolder = true;
                newParent.Route = string.Empty;
                newParent.ChildrenAsList = [newParentAsNode];
            }
            else
            {
                newParent = new PageNavigationModel
                {
                    Id = newParentPath,
                    Order = folderOrders.TryGetValue(newParentPath, out var order) ? order : 0,
                    Title = newParentPath,
                    IsFolder = true,
                    ChildrenAsList = [],
                };

                parent.ChildrenAsList.Add(newParent);
            }

            parent = newParent;
            parent.ChildrenAsList =
                parent
                    .ChildrenAsList?.OrderBy(a => a.Order)
                    ?.ThenBy(a => a.Title)
                    ?.ThenBy(a => a.ChildrenAsList != null)
                    ?.ToList() ?? [];
        }

        var pageMetadata = pageList.First(page => page.Key == path).Value;
        parent.ChildrenAsList.Add(
            // Add Node
            new PageNavigationModel
            {
                Id = node,
                Order = pageMetadata.Order,
                Title = pageMetadata.Title ?? node,
                Route = path,
            }
        );
        parent.ChildrenAsList =
            parent
                .ChildrenAsList?.OrderBy(a => a.Order)
                ?.ThenBy(a => a.Title)
                ?.ThenBy(a => a.ChildrenAsList != null)
                ?.ToList() ?? [];
    }
}
