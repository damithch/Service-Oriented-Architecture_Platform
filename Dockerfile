FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy solution and project files
COPY KMCEventPlatform.sln .
COPY src/KMCEventPlatform.Models/ src/KMCEventPlatform.Models/
COPY src/KMCEventPlatform.Data/ src/KMCEventPlatform.Data/
COPY src/KMCEventPlatform.Services/ src/KMCEventPlatform.Services/
COPY src/KMCEventPlatform.API/ src/KMCEventPlatform.API/

# Restore and build
RUN dotnet restore
RUN dotnet build -c Release

# Publish
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

# Expose ports
EXPOSE 80
EXPOSE 443

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost/health || exit 1

# Run application
ENTRYPOINT ["dotnet", "KMCEventPlatform.API.dll"]
