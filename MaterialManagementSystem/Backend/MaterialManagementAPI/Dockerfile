# Dockerfile for Material Management API (ASP.NET Core)
# Multi-stage build for optimized production image

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["MaterialManagementAPI.csproj", "."]
RUN dotnet restore "MaterialManagementAPI.csproj"

# Copy source code and build application
COPY . .
RUN dotnet build "MaterialManagementAPI.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "MaterialManagementAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published application
COPY --from=publish /app/publish .

# Create directories for file uploads
RUN mkdir -p wwwroot/uploads/invoices wwwroot/uploads/equipment

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose port
EXPOSE 80

# Health check endpoint
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl -f http://localhost/health || exit 1

# Set entry point
ENTRYPOINT ["dotnet", "MaterialManagementAPI.dll"]

