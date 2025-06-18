# Use the .NET 9.0 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the solution file and central package management files from the root
COPY *.sln .
COPY Directory.Packages.props . 
COPY Directory.Build.props .    

# Copy all project files from their respective directories
COPY src/Domain/*.csproj ./src/Domain/
COPY src/Application/*.csproj ./src/Application/
COPY src/Infrastructure/*.csproj ./src/Infrastructure/
COPY src/Web/*.csproj ./src/Web/
COPY tests/Application.FunctionalTests/*.csproj ./tests/Application.FunctionalTests/
COPY tests/Application.UnitTests/*.csproj ./tests/Application.UnitTests/
COPY tests/Domain.UnitTests/*.csproj ./tests/Domain.UnitTests/
COPY tests/Infrastructure.IntegrationTests/*.csproj ./tests/Infrastructure.IntegrationTests/

# Restore dependencies
RUN dotnet restore Backend.sln  # Explicitly specify the .sln file

# Copy the rest of the source code
COPY src/ ./src/

# Build and publish the Web project
WORKDIR /app/src/Web
RUN dotnet publish Web.csproj -c Release -o /app/out

# Use the .NET 9.0 runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Install netcat for waiting on PostgreSQL
RUN apt-get update && apt-get install -y netcat-openbsd

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# Expose port 80
EXPOSE 80

# Wait for PostgreSQL and run the app
ENTRYPOINT ["sh", "-c", "until nc -z db 5432; do sleep 1; done; dotnet Backend.Web.dll"]

