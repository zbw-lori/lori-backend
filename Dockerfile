# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY src/lori.backend.Infrastructure/*.csproj ./src/lori.backend.Infrastructure/
COPY src/lori.backend.Web/*.csproj ./src/lori.backend.Web/
RUN ls -laR /source
RUN dotnet restore

# copy everything else and build app
COPY src/. ./src/
WORKDIR /source/src/lori.backend.Infrastructure
RUN dotnet publish -c release -o /app/lori.backend.Infrastructure --no-restore
WORKDIR /source/src/lori.backend.Web
RUN dotnet publish -c release -o /app/lori.backend.Web --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "/app/lori.backend.Web/lori.backend.Web.dll"]