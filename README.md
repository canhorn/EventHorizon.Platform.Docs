# EventHorizon Platform Docs

# About 

This project is built on Blazor, with a focus on quick page creation. It includes a sidebar that will auto update based on razor files located in the Pages/Docs directory.

# Usage

The the only required step is to create a razor file with a [Page] attribute and inherit from the PageMetadataBase class.

Metadata feature:
- A [PageMetadata] attribute, this allow for setting meta information about the page from an attribute.
- Override Page Metadata with a jsonfile, it must match the format of [Razor File Name].razor.json
  - Example Razor File: GettingStarted.razor
  - Example Json File: GettingStarted.razor.json

# More Details

This project created a Side Navigation based on the routes of the pages, so it will nest the routes under "folder" creating a compact tree structure. 

The folders, pulled from the route, will require Resource Keys to be added to the SharedResource.resx to correctly localize.

## Json Format

When creating the json file, you can checkout the details below to get an idea of how the structure/support properties are.

~~~ json
{
    "Title": "Title of the Page",
    "CustomProperties": {
        "@comment": "CustomProperties are a string-string map of any support json characters.",
        "details":  "Some random details"
    }
}
~~~

# Example

You can clone this project and run the solution, checkout the GettingStarted.razor and CreateAMap.razor for examples of how the pages are structured. Any pages, correctly attributed, in the Pages directory should be supported.

# Creating a Docker Image

I have included a docker image, that can be used to package up the generated docs site for easy usage in just about any enjoinment.

~~~ bash
docker build -t <docker-org>/docs:latest .
~~~