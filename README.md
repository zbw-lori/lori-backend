# lori-backend

## set up database with `dotnet` cli

```shell
# install ef cli
dotnet tool install --global dotnet-ef
```

### create migration

```shell
dotnet ef migrations add $MIGRATION_NAME --project "src/lori.backend.Infrastructure" --startup-project "src/lori.backend.Web"
```

### update database

```shell
dotnet ef database update --project "src/lori.backend.Infrastructure" --startup-project "src/lori.backend.Web"
```

### generate sql script

```shell
dotnet ef migrations add $MIGRATION_NAME --project src/lori.backend.Infrastructure --startup-project src/lori.backend.Web
dotnet ef migrations script --output migrations.sql --project src/lori.backend.Infrastructure --startup-project src/lori.backend.Web
```

## Swagger UI

The Swagger UI can be found at https://localhost:57679/swagger

