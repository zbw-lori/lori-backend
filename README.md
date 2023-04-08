# lori-backend

## set up database with `dotnet` cli

### create migration

```shell
dotnet ef migrations add $MIGRATION_NAME --project "src/lori.backend.Infrastructure" --startup-project "src/lori.backend.Web"
```

### update database

```shell
dotnet ef database update --project "src/lori.backend.Infrastructure" --startup-project "src/lori.backend.Web"
```

## Swagger UI
The Swagger UI can be found at https://localhost:<port>/swagger
