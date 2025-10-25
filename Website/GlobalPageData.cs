namespace Website;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using Website.Metadata.Model;

// TODO: Could probably a Source Generator for this pattern,
//  where it finds all DatabaseKeys and generates this class automatically.
public static class GlobalPageDatabases
{
    public const string CustomPageDatabaseKey = "CustomPageData";

    [Pure]
    public static IDictionary<string, TemplatedRoutePage> GetPageRouteDatabaseByKey(
        string databaseKey
    )
    {
        return databaseKey switch
        {
            CustomPageDatabaseKey => GlobalPageDatabase.CustomPageData.ToDictionary(
                kvp => kvp.Key,
                kvp => (TemplatedRoutePage)kvp.Value
            ),
            _ => [],
        };
    }

    [Pure]
    public static IDictionary<string, TData>? GetPageDatabaseByKey<TData>(string databaseKey)
        where TData : TemplatedRoutePage
    {
        return databaseKey switch
        {
            CustomPageDatabaseKey => GlobalPageDatabase.CustomPageData
                as IDictionary<string, TData>,
            _ => new Dictionary<string, TData>(),
        };
    }

    [Pure]
    public static bool TryGetPageDatabaseByKey<TData>(
        string databaseKey,
        [NotNullWhen(true)] out IDictionary<string, TData>? database
    )
        where TData : TemplatedRoutePage
    {
        database = GetPageDatabaseByKey<TData>(databaseKey);
        return database != null;
    }
}

public static class GlobalPageDatabase
{
    public static readonly string PageRoutePrefix = "/custom-page/";

    public static readonly IDictionary<string, TemplateRouteData> CustomPageData = new Dictionary<
        string,
        TemplateRouteData
    >()
    {
        ["Custom Page 001"] = new TemplateRouteData(
            PageRoutePrefix,
            "Custom Page 001",
            "Custom Page 001",
            "An example page demonstrating custom content.",
            1.0f
        ),
        ["Custom Page 002"] = new TemplateRouteData(
            PageRoutePrefix,
            "Custom Page 002",
            "Custom Page 002",
            "Another example page demonstrating custom content.",
            2.0f
        ),
    };
}

public record TemplatedRoutePage
{
    public string Route { get; init; } = string.Empty;
    public PageMetadataModel Metadata { get; init; } = new PageMetadataModel();
}

public record TemplateRouteData : TemplatedRoutePage
{
    public string Id { get; }
    public string Title { get; }
    public string Description { get; }
    public float Order { get; }

    public TemplateRouteData()
    {
        Id = string.Empty;
        Title = string.Empty;
        Description = string.Empty;
        Order = 0;
    }

    public TemplateRouteData(
        string routePrefix,
        string id,
        string title,
        string description,
        float order
    )
    {
        Id = id;
        Title = title;
        Description = description;
        Order = order;

        Route = $"{routePrefix}{id}";
        Metadata = new PageMetadataModel()
        {
            Order = order,
            Route = Route,
            InNavigation = true,
            Title = title,
        };
    }
};
