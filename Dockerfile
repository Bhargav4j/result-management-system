# Stage 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder

WORKDIR /src

# Copy solution file
COPY RMS.sln ./

# Copy project files for dependency restoration (optimize layer caching)
COPY src/RMS.Web/RMS.Web.csproj ./src/RMS.Web/
COPY src/RMS.Application/RMS.Application.csproj ./src/RMS.Application/
COPY src/RMS.Infrastructure/RMS.Infrastructure.csproj ./src/RMS.Infrastructure/
COPY src/RMS.Domain/RMS.Domain.csproj ./src/RMS.Domain/
COPY RMS.Tests/RMS.Tests.csproj ./RMS.Tests/

# Restore NuGet packages
RUN dotnet restore RMS.sln

# Copy all source code
COPY . .

# Build the solution
RUN dotnet build RMS.sln -c Release --no-restore

# Publish the web application
RUN dotnet publish src/RMS.Web/RMS.Web.csproj -c Release -o /app/publish --no-restore --no-build

# Stage 2: Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Create non-root user for security
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copy published application from builder
COPY --from=builder /app/publish .

# Create directories for logs and data with proper permissions
RUN mkdir -p /app/logs /app/data && chown -R appuser:appuser /app

# Switch to non-root user
USER appuser

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://+:8080 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    TZ=UTC

# Expose application port
EXPOSE 8080

# Configure graceful shutdown
STOPSIGNAL SIGTERM

# Entry point
ENTRYPOINT ["dotnet", "RMS.Web.dll"]