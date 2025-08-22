namespace Website.Shared.Components;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

public partial class MarkdownFrom
{
    [Parameter]
    public required string ResourceName { get; set; }

    protected string Text { get; private set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (string.IsNullOrEmpty(ResourceName))
        {
            throw new ArgumentException("ResourceName must be provided.", nameof(ResourceName));
        }

        // Load markdown from Embedded Resources
        Text = EmbeddedResourceLoader.Load(ResourceName);
    }
}

public static class EmbeddedResourceLoader
{
    // TODO: To pull in multiple resources we should pull the Assemblies from PageMetadataSettings
    private static readonly HashSet<string> LoadedResources =
    [
        .. Assembly.GetExecutingAssembly().GetManifestResourceNames(),
    ];

    public static string Load(string resourceName)
    {
        var resourceFullName = LoadedResources.FirstOrDefault(r => r.EndsWith(resourceName));
        if (string.IsNullOrEmpty(resourceFullName))
        {
            throw new FileNotFoundException($"Resource not found: {resourceFullName}");
        }

        // Load the embedded resource as a string
        using var stream =
            Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceFullName)
            ?? throw new FileNotFoundException($"Resource not found: {resourceFullName}");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
