# ASP.NET Core Identity API Template

This repository is a reusable ASP.NET Core Web API template for .NET 8 applications that use ASP.NET Core Identity, Entity Framework Core, JWT authentication, refresh tokens, role seeding, and Swagger.

The project is configured as a `dotnet new` project template. When a project is generated from it, occurrences of `Identity_template` in namespaces, `using` statements, project filenames, and related files are replaced with the name supplied to `dotnet new`.

## Prerequisites

Install the following before using the template:

- .NET 8 SDK
- Visual Studio 2022/2026 or another .NET-compatible IDE
- SQL Server or another database configured for the application
- Entity Framework Core CLI tools

Install the EF Core CLI tool if it is not already available:

```powershell
dotnet tool install --global dotnet-ef
```

If it is already installed, update it when necessary:

```powershell
dotnet tool update --global dotnet-ef
```

## Clone the repository

Clone the repository and move into the template directory:

```powershell
git clone https://github.com/Victorjayce/asp.net_template.git
cd asp.net_template\Identity_template
```

Restore and build the template project if you want to inspect or run the template source directly:

```powershell
dotnet restore
dotnet build
```

## Install the template locally

From the directory that contains the `Identity_template` folder, install the template:

```powershell
cd ..
dotnet new install .\Identity_template
```

The template's short name is `identity-api`. Verify that it is installed:

```powershell
dotnet new list identity
```

You should see an entry for `identity-api` or `ASP.NET Core Identity API`.

## Create a new API from the template

Create a new project by supplying a valid C# project name:

```powershell
dotnet new identity-api -n OrdersApi -o .\OrdersApi
cd .\OrdersApi
dotnet restore
dotnet build
```

The generated project will use `OrdersApi` instead of `Identity_template`, including namespaces such as:

```csharp
namespace OrdersApi.Services
```

Use a name that is also a valid C# identifier. Names such as `OrdersApi` and `InventoryApi` are recommended. Avoid names containing spaces or hyphens because the project name is used in namespaces.

## Configure appsettings

Before running the generated API, update the configuration files:

- `appsettings.json` for shared or non-secret defaults
- `appsettings.Development.json` for local development settings

At minimum, check and configure the following sections used by the application:

- Database connection string
- JWT issuer and audience
- JWT signing key
- Token expiration settings, if present
- Logging and allowed hosts, if required

Example connection string format:

```json
{
  "ConnectionStrings": {
	"DefaultConnection": "Server=localhost;Database=OrdersApiDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Use the actual connection-string key expected by `DataContext` and `Program.cs`. Do not commit production passwords, connection strings, JWT signing keys, or other secrets to source control.

For local development, prefer user secrets or environment variables:

```powershell
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your-connection-string"
dotnet user-secrets set "Jwt:Key" "your-development-signing-key"
```

The exact JWT configuration key must match the key names read by the generated application's configuration code.

## Create Entity Framework migrations

Migrations are intentionally excluded from generated projects by `.template.config/template.json`. Create a new migration for each generated project after configuring its database connection:

```powershell
dotnet ef migrations add InitialCreate
dotnet ef database update
```

This creates a new `Migrations` directory and applies the schema to the configured database.

For later model changes, create another migration and update the database:

```powershell
dotnet ef migrations add AddNewModelChange
dotnet ef database update
```

If the startup project or `DbContext` cannot be detected, specify them explicitly:

```powershell
dotnet ef migrations add InitialCreate --context DataContext --project . --startup-project .
dotnet ef database update --context DataContext --project . --startup-project .
```

Review generated migrations before applying them, especially when working with an existing database.

## Run the API

After configuring the database and applying migrations, run the API:

```powershell
dotnet run
```

Alternatively, start it from Visual Studio using the generated project's launch profile. Swagger is available at the URL displayed in the console, commonly:

```text
https://localhost:<port>/swagger
```

The API seeds configured roles when the application starts, according to the implementation in `SeedRoles.cs`.

## Edit the template for future projects

The template definition is located at:

```text
.template.config\template.json
```

The most important properties are:

- `shortName`: the command used with `dotnet new`
- `name`: the display name shown by `dotnet new list`
- `sourceName`: the text replaced by the generated project name
- `exclude`: files and folders that should not be copied into generated projects

After changing `template.json` or any template source files, reinstall the local template. From the directory containing `Identity_template` run:

```powershell
dotnet new install .\Identity_template --force
```

Then create a fresh test project:

```powershell
dotnet new identity-api -n TemplateTestApi -o .\TemplateTestApi
dotnet build .\TemplateTestApi\TemplateTestApi.csproj
```

Always test a newly generated project after changing the template. Do not test only the template source project because the generated project also verifies namespace and filename replacement.

## Update or uninstall the local template

List installed templates:

```powershell
dotnet new list
```

Reinstall the local version after changes:

```powershell
dotnet new install .\Identity_template --force
```

To uninstall it, use the template identity shown by `dotnet new list`:

```powershell
dotnet new uninstall Victorjayce.AspNetCore.IdentityApi
```
