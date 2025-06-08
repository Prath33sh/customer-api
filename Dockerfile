# Build stage for main project and tests
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and projects
COPY customer-api.sln ./
COPY customer-api/customer-api.csproj ./customer-api/
COPY customer-api.tests/customer-api.tests.csproj ./customer-api.tests/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY customer-api ./customer-api
COPY customer-api.tests ./customer-api.tests

# Build and publish the main project
RUN dotnet publish ./customer-api/customer-api.csproj -c Release -o /app/publish

# Optional: Run tests (uncomment if you want to run tests in the build)
# RUN dotnet test ./customer-api.tests/customer-api.tests.csproj --no-build --verbosity normal

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "customer-api.dll"]