FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env 
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY Messages/*.csproj ./Messages/
COPY Service/*.csproj ./Service/
RUN dotnet restore ./Service/

# Copy everything else and build
COPY Messages/. ./Messages/
COPY Service/. ./Service/
WORKDIR /app/Service
RUN dotnet publish -c Release -o ../out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "MassTransitStudies.Service.dll"]