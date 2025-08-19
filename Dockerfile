# Stage 1 - Restore .NET Project
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

COPY ./*.sln ./

# Copy Main Source
COPY Website/Website.csproj ./Website/Website.csproj

# Restore Packages
RUN dotnet restore

# Build from Source
RUN dotnet build -c Release --no-restore

## Single folder publish of whole solution
RUN dotnet publish --output /app/ --configuration Release --no-restore --no-build


# Stage 2 - Runtime
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Website.dll"]
