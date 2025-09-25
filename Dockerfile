# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution + props
COPY *.sln .
COPY Directory.Packages.props .
COPY Directory.Build.props .

# Copy projects
COPY src/Domain/*.csproj ./src/Domain/
COPY src/Application/*.csproj ./src/Application/
COPY src/Infrastructure/*.csproj ./src/Infrastructure/
COPY src/Web/*.csproj ./src/Web/

# Copy test projects (optional)
COPY tests/Application.FunctionalTests/*.csproj ./tests/Application.FunctionalTests/
COPY tests/Application.UnitTests/*.csproj ./tests/Application.UnitTests/
COPY tests/Domain.UnitTests/*.csproj ./tests/Domain.UnitTests/
COPY tests/Infrastructure.IntegrationTests/*.csproj ./tests/Infrastructure.IntegrationTests/

# Restore dependencies
RUN dotnet restore Backend.sln

# Copy everything
COPY . .

# Publish Web project
WORKDIR /src/Web
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Install netcat for waiting on DB
RUN apt-get update && apt-get install -y --no-install-recommends netcat-openbsd && rm -rf /var/lib/apt/lists/*

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5000

EXPOSE 5000

# Wait for PostgreSQL then run app
ENTRYPOINT ["sh", "-c", "until nc -z db 5432; do sleep 1; done; dotnet Backend.Web.dll"]
