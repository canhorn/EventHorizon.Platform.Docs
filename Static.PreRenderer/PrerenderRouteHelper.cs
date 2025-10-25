namespace Static.PreRenderer;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Website;
using Website.Metadata.Attributes;

public static class PrerenderRouteHelper
{
    public static List<string> GetRoutes(Assembly assembly)
    {
        // Get all the components whose base class is ComponentBase
        var components = assembly.ExportedTypes.Where(t => t.IsSubclassOf(typeof(ComponentBase)));

        var routes = components
            .Select(GetRouteFromComponents)
            .SelectMany(route => route)
            .Where(config => config is not null)
            .ToList();

        return routes;
    }

    private static string[] GetRouteFromComponents(Type component)
    {
        var attributes = component.GetCustomAttributes(inherit: true);

        return
        [
            .. attributes
                .OfType<RouteAttribute>()
                .SelectMany(route => GetRouteFromComponent(route, component)),
        ];
    }

    private static List<string> GetRouteFromComponent(RouteAttribute routeAttribute, Type component)
    {
        if (routeAttribute is null)
        {
            // Only map routable components
            return null;
        }

        var route = routeAttribute.Template;

        if (string.IsNullOrEmpty(route))
        {
            throw new Exception(
                $"RouteAttribute in component '{component}' has empty route template"
            );
        }

        // If the route contains route values, check for PageMetadataAttribute and DatabaseKey
        if (route.Contains('{'))
        {
            // Get the Data Attribute From the Component
            var attributes = component
                .GetCustomAttributes(inherit: true)
                .OfType<PageMetadataAttribute>();
            if (!attributes.Any())
            {
                return [];
            }

            var pageMetadataAttribute = attributes.First();
            if (string.IsNullOrEmpty(pageMetadataAttribute.DatabaseKey))
            {
                return [];
            }

            // Get the Routes from the Page Database
            return
            [
                .. GlobalPageDatabases
                    .GetPageRouteDatabaseByKey(pageMetadataAttribute.DatabaseKey)
                    .Values.Where(data => !string.IsNullOrEmpty(data.Route))
                    .Select(data => data.Route),
            ];
        }

        return [route];
    }
}
