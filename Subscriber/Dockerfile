FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY Messages/*.csproj ./Messages/
COPY Subscriber/*.csproj ./Subscriber/
RUN dotnet restore ./Subscriber/

# Copy everything else and build
COPY Messages/. ./Messages/
COPY Subscriber/. ./Subscriber/
WORKDIR /app/Subscriber
RUN dotnet publish -c Release -o ../out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "MassTransitStudies.Subscriber.dll"]