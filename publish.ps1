Remove-Item output -force -recurse
dotnet restore
dotnet clean

# publish the app
dotnet publish -c Release -o output Website

# Set the render output (use $PSScriptRoot instead of $pwd inside of *.ps1 script files)
$env:RenderOutputDirectory="$pwd/output/wwwroot"

# Generate the output
dotnet test -c Release --filter Category=PreRender
