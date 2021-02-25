namespace EventHorizon.Platform.Docs.Metadata.State
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json;
    using EventHorizon.Platform.Docs.Metadata.Api;
    using EventHorizon.Platform.Docs.Metadata.Attributes;
    using EventHorizon.Platform.Docs.Metadata.Model;
    using Microsoft.AspNetCore.Components;

    public class StandardPageMetadataRepository
        : PageMetadataRepository
    {
        private readonly ConcurrentDictionary<string, PageMetadataModel> _map;
        private readonly PageNavigation _nav;

        public StandardPageMetadataRepository(
            PageMetadataSettings settings
        )
        {
            _map = GenerateListOfPageMetadata(
                settings
            );
            _nav = BuildPageNavigation(
                _map
            );
        }

        public IEnumerable<PageMetadataModel> All()
        {
            return _map.Values;
        }

        public PageMetadataModel Get(
            string route
        )
        {
            return _map[route];
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
            var pageMetadataList = settings.PageAssemblyList
                .SelectMany(x => x.DefinedTypes)
                .Where(type => typeof(PageMetadata).IsAssignableFrom(type));

            foreach (var typeInfo in pageMetadataList)
            {
                if (Attribute.IsDefined(
                    typeInfo,
                    typeof(PageAttribute),
                    false
                ))
                {
                    var pageAttribute = typeInfo.GetCustomAttribute<PageAttribute>();
                    var routeAttribute = typeInfo.GetCustomAttribute<RouteAttribute>();
                    pageFileNameList.Add(
                        typeInfo.Name
                    );
                    if (routeAttribute == null)
                    {
                        throw new SystemException(
                            $"Page Metadata needs a RouteAttribute to function: {typeInfo.Name}"
                        );
                    }
                    var model = new PageMetadataModel
                    {
                        Title = typeInfo.Name,
                    };
                    // If has the PageMetadata Attribute pull data from that first
                    if (Attribute.IsDefined(
                        typeInfo,
                        typeof(PageMetadataAttribute)
                    ))
                    {
                        var pageMetadataAttribute = typeInfo.GetCustomAttribute<PageMetadataAttribute>();
                        model.Title = pageMetadataAttribute?.Title ?? typeInfo.Name;
                    }

                    model.Route = routeAttribute.Template;

                    pageList.Add(
                        routeAttribute.Template,
                        model
                    );
                }
            }

            return new ConcurrentDictionary<string, PageMetadataModel>(
                pageList
            );
        }

        private static PageNavigation BuildPageNavigation(
            IDictionary<string, PageMetadataModel> pageList
        )
        {
            var pathList = pageList.Keys;
            var root = new PageNavigationModel
            {
                Id = "root",
                Title = "root",
                IsFolder = true,
                ChildrenAsList = new List<PageNavigationModel>(),
            };

            foreach (var path in pathList)
            {
                AddNavigationModelToNode(
                    pageList,
                    root,
                    path
                );
            }
            root.ChildrenAsList = root.ChildrenAsList?.OrderBy(
                a => a.Title
            )?.OrderBy(
                a => a.ChildrenAsList != null
            )?.ToList() ?? new List<PageNavigationModel>();

            return root;
        }

        private static void AddNavigationModelToNode(
            IDictionary<string, PageMetadataModel> pageList,
            PageNavigationModel root,
            string path
        )
        {
            var splitPath = path.Split(
                "/",
                StringSplitOptions.RemoveEmptyEntries
            ).ToList();
            var node = splitPath.Last();
            var parentList = splitPath.Take(
                splitPath.Count - 1
            );
            var parent = root;

            foreach (var newParentPath in parentList)
            {
                var newParent = default(PageNavigationModel);

                if (parent.Children.Any(
                    a => a.Id == newParentPath
                ) && parent.Children.First(
                    a => a.Id == newParentPath
                ).IsFolder)
                {
                    // Set newParent to already existing 
                    newParent = (PageNavigationModel)parent.Children.First(
                        a => a.Id == newParentPath
                    );
                }
                else if (parent.Children.Any(
                    a => a.Id == newParentPath
                ) && !parent.Children.First(
                    a => a.Id == newParentPath
                ).IsFolder)
                {
                    // Update Parent Node to Parent Folder
                    // And add Node to Folder
                    newParent = (PageNavigationModel)parent.Children.First(
                        a => a.Id == newParentPath
                    );
                    var newParentAsNode = new PageNavigationModel
                    {
                        Id = newParent.Id,
                        Title = newParent.Title,
                        Route = newParent.Route,
                    };
                    // Set Parent to Folder
                    newParent.IsFolder = true;
                    newParent.Route = string.Empty;
                    newParent.ChildrenAsList = new List<PageNavigationModel>
                    {
                        newParentAsNode
                    };
                }
                else
                {
                    newParent = new PageNavigationModel
                    {
                        Id = newParentPath,
                        Title = newParentPath,
                        IsFolder = true,
                        ChildrenAsList = new List<PageNavigationModel>(),
                    };

                    parent.ChildrenAsList.Add(newParent);
                }

                parent = newParent;
                parent.ChildrenAsList = parent.ChildrenAsList?.OrderBy(
                    a => a.Title
                )?.OrderBy(
                    a => a.ChildrenAsList != null
                )?.ToList() ?? new List<PageNavigationModel>();
            }

            parent.ChildrenAsList.Add(
                // Add Node
                new PageNavigationModel
                {
                    Id = node,
                    Title = pageList.First(
                        page => page.Key == path
                    ).Value.Title ?? node,
                    Route = path,
                }
            );
            parent.ChildrenAsList = parent.ChildrenAsList?.OrderBy(
                a => a.Title
            )?.OrderBy(
                a => a.ChildrenAsList != null
            )?.ToList() ?? new List<PageNavigationModel>();
        }
    }
}
