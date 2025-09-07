# EventHorizon Docs

## About

This project is built on Blazor, with a focus on quick page creation.

## Features

- Sidebar that will auto update based on razor annotated with the [PageAttribute] Attribute.
- Markdown can be pulled from `md` embedded files
- Static Site Generation for Blazor Wasm
- Localization Built-In
- Utility Components Available for Markdown and general debugging display.

## Support

This project supports Blazor Server, Wasm, and Static Site Wasm deployments.

## Getting Started

- Update `BasePath` in `Server/appsettings.json` and `Website/wwwroot/appsettings.json` to your Hosting URL's Base Path.
  - Controls the base URL for your site, including GitHub Pages or other hosting providers.
  - This is not necessary if your deploying to a host that has no subdirectory or is being removed during routing.
  - If your deploying to GitHub set this to the GitHub Pages Path URL.
  - If your using a custom website that is deployed at the root you can ignore the configuration.
  - For local development create a `Server/appsettings.Development.json` and `Website/wwwroot/appsettings.Development.json` file with the following content:

  ```json
  {
    "BasePath": ""
  }
  ```

- Update `Website/wwwroot/404.html` to include the correct base path for GitHub.
  - Not necessary if your site is hosted at the root.
- Update or Replace values from `Website/Shared/GlobalPageConfig.razor.cs` to set the correct values for your site.
  - `PrivacyPolicyEmail` - The email address for your site's privacy policy.
  - `DocsUrl` - The URL of your site.
  - `DocsSite` - The Name of your site.
  - `DocsDescription` - The Description of your site.

## Usage

The the only required step is to create a razor file with a [Page] attribute and inherit from the PageMetadataBase class.

## Metadata

Including [PageMetadata] attribute, set meta information about the page.

### Supported Metadata

| Property | Description                                     |
| -------- | ----------------------------------------------- |
| Title    | Used for display purposes outside of this page. |

## Example Razor Files

### Razor Page

Create a Razor page with the [Page] attribute and inherit from PageMetadataBase to set metadata.

Map.razor

```html
@page "/examples/a-map" @attribute [Page] @attribute [PageMetadata(Title
= "Map")] @inherits PageMetadataBase

<h1>@Localizer["Map"]</h1>

<p>@Localizer["Tutorial on how to create a map..."]</p>
```

### Markdown Page

Markdown registered in the `Website/Pages` folder will be automatically be registered as an embedded resources. That can then be referenced by name using the `MarkdownFrom` component.

MarkdownPage.razor

```html
@page "/examples/markdown"
@attribute [Page]
@attribute [PageMetadata(Title = "Markdown Example")]
@inherits PageMetadataBase

<MarkdownFrom ResourceName="MarkdownPage.razor.md" />

```

MarkdownPage.razor.md

```md
# Markdown Example

This is an example of a markdown page.

```

## Application Details

This project created a Side Navigation based on the routes of the pages, so it will nest the routes under "folder" creating a compact tree structure.

The folders, pulled from the route, will require Resource Keys to be added to the SharedResource.resx to correctly localize.

## Deployment Scenarios

You can clone this project and run the solution, checkout the `Index.Razor`, `Map.razor` and `MarkdownPage.razor` for examples of how pages are structured. Any pages, including pages not explicitly mentioned, in the Pages directory should be supported.

## Creating a Blazor Server Hosted Docker Image

I have included a Dockerfile, that can be used to package up the generated docs site for easy usage in just about any environment.

```bash
docker build -t <docker-org>/docs:latest .
```

## Push a Built Docker Image to the Docker Hub Registry

```bash
docker push <docker-org>/docs:latest
```

## Generate Static Pre-Rendered Output Files

This process uses the Static.PreRenderer project to spin up an InMemory Host of the Server Project, that that then goes through all the registered Routes generating a base, a gzipped compressed and a brotli compressed version of the page into the output/wwwroot folder.

After running the command you can find the generate source in the ./output/wwwroot folder, you can then take that generated source and publish it into any static host provider.

```bash
# Linux: Using sh you can generate the files
sh publish.sh
```

```powershell
# Windows: Using Powershell you can generate the files
./publish.ps1
```

```bash
# Use the dotnet serve tool to host a static version of the site
dotnet serve -d "./output/wwwroot"
```

```bash
# Install .NET Serve
dotnet tool install --global dotnet-serve

# Install wasm-tools workload
dotnet workload install wasm-tools
```

Inspiration for the Pre-Rendering was from the blog of Andrew Lock. The post most of the Pre-Renderer was derived from is here <https://andrewlock.net/enabling-prerendering-for-blazor-webassembly-apps/>
