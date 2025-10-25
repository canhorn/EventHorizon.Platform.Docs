namespace Website.Metadata.Model;

using Microsoft.AspNetCore.Components;

public abstract class DatabasePageMetadataBase<TData> : PageMetadataBase
    where TData : TemplatedRoutePage, new()
{
    [Parameter]
    public required string Slug { get; set; }

    public abstract string DatabaseKey { get; }

    protected TData Data { get; set; } = new();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (string.IsNullOrEmpty(Slug))
        {
            return;
        }

        if (
            GlobalPageDatabases.TryGetPageDatabaseByKey<TData>(DatabaseKey, out var database)
            && database.TryGetValue(Slug, out var data)
        )
        {
            Data = data;

            OverridePageMetadata(data.Metadata);
        }
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (string.IsNullOrEmpty(Slug))
        {
            return;
        }

        if (
            GlobalPageDatabases.TryGetPageDatabaseByKey<TData>(DatabaseKey, out var database)
            && database.TryGetValue(Slug, out var data)
        )
        {
            Data = data;

            OverridePageMetadata(data.Metadata);
        }
    }
}
