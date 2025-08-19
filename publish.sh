# store the solution directory as a variable
SLN_DIR="$(pwd)"
rm -rf output
dotnet restore
dotnet clean

# Build and publish the WebAssembly app
dotnet publish -c Release -o "output" ./Website

# Set the RenderOutputDirectory environment variable
# and run the Prerender test to generate the output
RenderOutputDirectory="${SLN_DIR}/output/wwwroot" \
dotnet test -c Release --filter Category=PreRender 
