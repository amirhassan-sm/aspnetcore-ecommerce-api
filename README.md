# Shopping

**A Clean Architecture ASP.NET Core 8 backend for catalogue and store management.**

## About

Shopping is a modular Web API that models the core of an e-commerce catalogue: hierarchical categories, products with pricing and SEO, reusable feature attributes, and a full identity layer for users and roles.

The solution is organized around Clean Architecture boundaries — domain, application, infrastructure, and API — so business rules stay independent of persistence and transport concerns. Authentication is handled with ASP.NET Core Identity and JWT (access + refresh tokens). Application services are covered by unit tests, and Swagger documents the API surface for local development.

This project is intended as a solid reference implementation and portfolio piece for building maintainable .NET backends with clear separation of concerns.

## Features

- **Catalogue** — hierarchical categories, products, SEO metadata, stock & pricing
- **Features** — reusable product attributes; assign to categories or products and set values
- **Auth** — ASP.NET Core Identity + JWT (access + refresh tokens)
- **Admin** — user, role, and role-assignment management
- **API docs** — Swagger UI in Development
- **Tests** — xUnit unit tests for application services (Moq)

## Tech Stack

| Layer | Technology |
|-------|------------|
| Runtime | .NET 8 / ASP.NET Core Web API |
| ORM | Entity Framework Core 8 + SQL Server |
| Auth | Identity + JWT Bearer |
| Docs | Swashbuckle (Swagger / OpenAPI) |
| Tests | xUnit, Moq |

## Architecture

```text
Shopping (API host)
        │
        ▼
Application  ←→  Application.Contrast (contracts)
        │         Application.DTOs
        ▼
Catalogue.Domain  ←→  DomainServiceContract
        │
        ▼
Infrastructure (EF Persistence + Security)
```

| Project | Responsibility |
|---------|----------------|
| `Shopping` | Web API host — controllers, DI, JWT, Swagger |
| `Application` | Use cases / application services |
| `ApplicationServiceContract` | Application interfaces & query contracts |
| `Application.DTOs` | DTOs and Identity models |
| `FramWork.Application` | Shared result types & pagination |
| `Catalogue.Domain` | Catalogue entities |
| `Catalogue.DomainServiceContract` | Repository abstractions |
| `Framwork.Domain` | Generic repository base |
| `Infrastracture.Catalogue.EF.Persistance` | EF Core, repos, queries, migrations |
| `Security` | Identity DbContext, JWT auth, user management |
| `Application.Tests` | Unit tests for application layer |

**Patterns used:** Repository, Application Service, DTO, Result Object (`OperationResult`), light CQRS (command repos vs query services), DI, role-based authorization.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB, Express, or full) with Windows Authentication (or update connection strings)

## Getting Started

### 1. Clone & restore

```bash
git clone https://github.com/<your-username>/<your-repo>.git
cd Shopping
dotnet restore
```

### 2. Configure connection strings

Edit `Shopping/appsettings.json` (or use [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) / environment variables):

```json
{
  "ConnectionStrings": {
    "CatalogueConnectionString": "Data Source=.;Initial Catalog=Shopping;Integrated Security=True;Trust Server Certificate=True",
    "Security": "Data Source=.;Initial Catalog=Security;Integrated Security=True;Trust Server Certificate=True"
  },
  "jwt": {
    "SecretKey": "change-me-to-a-long-random-secret",
    "DurationInMinutes": 60
  }
}
```

Two databases are used:

- **Shopping** — catalogue (`CatalogueContext`)
- **Security** — users & roles (`SecurityContext`)

> **Security note:** Do not commit real production secrets. Prefer User Secrets or environment variables for `jwt:SecretKey` and connection strings.

### 3. Apply migrations

```bash
dotnet ef database update --project Infrastracture.Catalogue.EF.Persistance --startup-project Shopping --context CatalogueContext
dotnet ef database update --project Security --startup-project Shopping --context SecurityContext
```

If `dotnet ef` is missing:

```bash
dotnet tool install --global dotnet-ef
```

### 4. Run the API

```bash
dotnet run --project Shopping
```

| Profile | URL |
|---------|-----|
| HTTP | http://localhost:5250 |
| HTTPS | https://localhost:7002 |
| Swagger | http://localhost:5250/swagger |

## API Overview

### Authentication — `api/Authentication`

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/signUp` | Register |
| POST | `/Login` | Login (JWT) |
| POST | `/Refresh` | Refresh token |

### Products — `api/Product`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/SearchProduct` | Search / list |
| POST | `/AddProduct` | Create |
| GET | `/{id}` | Get by id |
| PUT | `/Update` | Update |
| DELETE | `/{id}` | Delete |
| PUT | `/SetSeoModel` | Set SEO |
| GET | `/GetProductFeatures` | List features |
| POST | `/assigneFeatureToProduct/{ProductId}/{FeatureId}` | Assign feature |
| DELETE | `/RemoveFeatureFromProduct/{ProductId}/{FeatureId}` | Remove feature |
| PUT | `/SetFeaturesValue` | Set feature values |

### Features — `api/Feature`

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/add` | Create |
| GET | `/{id}` | Get by id |
| GET | `/Search` | Search |
| PUT | `/UpdateFeature` | Update |
| DELETE | `/{id}` | Delete |

### Categories (Admin) — `api/admin/CategoryAdminManagement`

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/AddNewCategory` | Create |
| GET | `/GetCategory/{id}` | Get by id |
| GET | `/SearchCategory` | Search |
| GET | `/GetLevel1Category` | Top-level categories |
| PUT | `/UpdateCategory` | Update |
| DELETE | `/RemoveCategory` | Delete |
| POST | `/AssignFeatureToCategory` | Assign feature |
| DELETE | `/RemoveFeatureFromCategory/{FeatureId}/{CategoryId}` | Remove feature |
| GET | `/GetFeatures/{id}` | Category features |

### Users & Roles (Admin)

| Controller | Base route | Notes |
|------------|------------|--------|
| UserManagement | `UserManagement` | Profile, list users, delete — `[Authorize(Roles="admin")]` |
| RoleManager | `RoleManager` | CRUD roles — admin |
| UserRoles | `api/UserRoles` | Assign / remove roles — admin |

Authorize in Swagger with: `Bearer {your_access_token}`.

## Running Tests

```bash
dotnet test Application.Tests/Application.Tests.csproj
```

Tests cover Category, Product, Feature, and User application services with mocked dependencies (no database required).

## Project Structure

```text
Shopping/
├── Application/                          # Use cases
├── Application.DTOs/                     # DTOs & Identity models
├── ApplicationServiceContract/           # Application interfaces
├── Application.Tests/                    # Unit tests
├── Catalogue.Domain/                     # Domain entities
├── Catalogue.DomainServiceContract/      # Repository contracts
├── FramWork.Application/                 # Shared application helpers
├── Framwork.Domain/                      # Generic repo base
├── Infrastracture.Catalogue.EF.Persistance/
├── Security/                             # Identity + JWT
└── Shopping/                             # API host
    ├── Controllers/
    ├── Program.cs
    └── appsettings.json
```

## License

This project is provided as-is for learning and portfolio use. Add a license file (e.g. MIT) if you plan to open-source it formally.
