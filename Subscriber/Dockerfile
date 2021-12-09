FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env 
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
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "MassTransitStudies.Subscriber.dll"]