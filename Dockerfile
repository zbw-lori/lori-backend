# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0.202 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY . ./
RUN dotnet restore

# copy everything else and build app
WORKDIR /source/src

RUN dotnet publish ./lori.backend.Web -c release --no-restore
RUN mv /source/src/lori.backend.Web/bin/release/net7.0/publish/ /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "/app/lori.backend.Web.dll"]
EXPOSE 80