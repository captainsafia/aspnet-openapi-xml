# Use the most recent .NET LTS as the base image
FROM mcr.microsoft.com/devcontainers/dotnet:1-8.0

# Install the current .NET STS release on top of that
COPY --from=mcr.microsoft.com/dotnet/sdk:9.0 /usr/share/dotnet /usr/share/dotnet

# Finally install the most recent .NET 10.0 preview using the dotnet-install script
COPY --from=mcr.microsoft.com/dotnet/nightly/sdk:10.0.100-preview.2 /usr/share/dotnet /usr/share/dotnet
