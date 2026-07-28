# Shopping — ASP.NET Core E-Commerce API

**A Clean Architecture ASP.NET Core 8 backend for catalogue management and online sales.**

> Portfolio / resume project — modular Web API with clear domain boundaries, JWT auth, and a working checkout flow.

**Frontend (Next.js):** https://github.com/amirhassan-sm/Ecommerce-FrontEnd.git

## About

Shopping is a modular Web API that models a **classic online store**: hierarchical categories, products with pricing/SEO/stock, reusable feature attributes, product media (images/videos), and a sale flow (basket → order → stock deduction).

The solution follows Clean Architecture — domain, application, infrastructure, and API — so business rules stay independent of persistence and transport. Authentication uses ASP.NET Core Identity with JWT (access + refresh). Role-based authorization separates **Admin** (catalogue management) from **User** (browse & buy). FluentValidation validates incoming DTOs, and Swagger documents the API for local development.

## Features

- **Catalogue** — hierarchical categories, products, SEO metadata, stock & pricing
- **Features** — reusable attributes; assign to categories/products and set values
- **Product media** — upload images/videos, ordered display, confirm/unconfirm, file storage
- **Sale / checkout** — enrich basket from server prices, register order, deduct stock, cancel & restore stock
- **Auth** — Identity + JWT (access + refresh); roles seeded on startup (`admin`, `User`)
- **Authorization** — public browse; signed-in users buy; admins manage catalogue & users
- **Validation** — FluentValidation pipeline filter on controllers
- **API docs** — Swagger UI in Development
- **Tests** — xUnit unit tests for application services (Moq)

## Tech Stack

| Layer | Technology |
|-------|------------|
| Runtime | .NET 8 / ASP.NET Core Web API |
| ORM | Entity Framework Core 8 + SQL Server |
| Auth | Identity + JWT Bearer |
| Validation | FluentValidation |
| Docs | Swashbuckle (Swagger / OpenAPI) |
| Tests | xUnit, Moq |

## Architecture

```text
Shopping (API host)
        │
        ▼
Application  ←→  Application.Contrast (contracts)
        │         Application.DTOs
        ├──────────────────┐
        ▼                  ▼
Catalogue.Domain      Sale.Domain
        │                  │
        ▼                  ▼
Catalogue EF Persist.  Infrastructure.Sale
        │
        ▼
Security (Identity + JWT)
```

Three databases keep concerns separated:

| Database | Context | Purpose |
|----------|---------|---------|
| **Shopping** | `CatalogueContext` | Categories, products, features, media |
| **Security** | `SecurityContext` | Users, roles, tokens |
| **SaleSystem** | `SaleSystemContext` | Orders & order details |

| Project | Responsibility |
|---------|----------------|
| `Shopping` | Web API host — controllers, DI, JWT, Swagger, role seeding |
| `Application` | Use cases (catalogue, media, order management) |
| `ApplicationServiceContract` | Application & query interfaces |
| `Application.DTOs` | DTOs and Identity models |
| `FramWork.Application` | Shared result types (`OperationResult`, pagination) |
| `Catalogue.Domain` | Catalogue entities |
| `Catalogue.DomainServiceContract` | Catalogue repository contracts |
| `Infrastracture.Catalogue.EF.Persistance` | EF repos, queries, file storage, migrations |
| `Sale.Domain` | Order / OrderDetails entities |
| `Sale.DomainServiceContract` | Order repository contracts |
| `Infrastructure.Sale` | Sale EF context, order repository, migrations |
| `Security` | Identity DbContext, JWT auth, user management |
| `Application.Tests` | Unit tests for application layer |

**Patterns:** Repository, Application Service, DTO, Result Object, light CQRS (command repos vs query services), DI, role-based authorization.

## Roles (classic shop)

| Role | Can do |
|------|--------|
| **Guest** | Browse products/categories/media, preview basket |
| **User** | Register/login, place orders, view/cancel own orders, profile |
| **Admin** | Full catalogue CRUD (product, feature, category, media), roles & users |

On startup, roles `admin` and `User` are seeded. New sign-ups receive the `User` role automatically. Assign `admin` once via `api/UserRoles` (or the database) for your first admin account.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB, Express, or full) with Windows Authentication (or update connection strings)

## Getting Started

### 1. Clone & restore

```bash
git clone https://github.com/amirhassan-sm/aspnetcore-ecommerce-api.git
cd aspnetcore-ecommerce-api
dotnet restore
```

### 2. Configure connection strings

Edit `Shopping/appsettings.json` (or use [User Secrets](https://learn.microsoft.com/en-us/aspnetcore/security/app-secrets) / environment variables):

```json
{
  "ConnectionStrings": {
    "CatalogueConnectionString": "Data Source=.;Initial Catalog=Shopping;Integrated Security=True;Trust Server Certificate=True",
    "Security": "Data Source=.;Initial Catalog=Security;Integrated Security=True;Trust Server Certificate=True",
    "Sale": "Data Source=.;Initial Catalog=SaleSystem;Integrated Security=True;Trust Server Certificate=True"
  },
  "jwt": {
    "SecretKey": "change-me-to-a-long-random-secret",
    "DurationInMinutes": 60
  }
}
```

> **Security note:** Do not commit real production secrets. Prefer User Secrets or environment variables for `jwt:SecretKey` and connection strings.

### 3. Apply migrations

```bash
dotnet ef database update --project Infrastracture.Catalogue.EF.Persistance --startup-project Shopping --context CatalogueContext
dotnet ef database update --project Security --startup-project Shopping --context SecurityContext
dotnet ef database update --project Infrastructure.Sale --startup-project Shopping --context SaleSystemContext
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

Authorize in Swagger with: `Bearer {your_access_token}`.

## API Overview

### Authentication — `api/Authentication`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/signUp` | Public | Register (+ role `User`) |
| POST | `/Login` | Public | Login (JWT) |
| POST | `/Refresh` | Public | Refresh token |

### Products — `api/Product`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/SearchProduct` | Public | Search / list |
| GET | `/{id}` | Public | Get by id |
| GET | `/GetProductFeatures` | Public | List product features |
| POST | `/AddProduct` | Admin | Create |
| PUT | `/Update` | Admin | Update |
| DELETE | `/{id}` | Admin | Delete |
| PUT | `/SetSeoModel` | Admin | Set SEO |
| POST | `/assigneFeatureToProduct/...` | Admin | Assign feature |
| DELETE | `/RemoveFeatureFromProduct/...` | Admin | Remove feature |
| PUT | `/SetFeaturesValue` | Admin | Set feature values |

### Product media — `api/ProductMedia`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/GetMedia/{id}` | Public | Get media by id |
| GET | `/GetAllProductMediaByproductId/{productId}` | Public | All media |
| GET | `/GetImagesByProductId/{productId}` | Public | Images |
| GET | `/GetVideosByProductId/{productId}` | Public | Videos |
| POST | `/AddNewMedia` | Admin | Upload media (`multipart/form-data`) |
| DELETE | `/{ProductMediaId}` | Admin | Remove media |

### Features — `api/Feature`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/{id}` | Public | Get by id |
| GET | `/Search` | Public | Search |
| POST | `/add` | Admin | Create |
| PUT | `/UpdateFeature` | Admin | Update |
| DELETE | `/{id}` | Admin | Delete |

### Categories — `api/admin/CategoryAdminManagement`

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/GetCategory/{id}` | Public | Get by id |
| GET | `/SearchCategory` | Public | Search |
| GET | `/GetLevel1Category` | Public | Top-level categories |
| GET | `/GetFeatures/{id}` | Public | Category features |
| POST | `/AddNewCategory` | Admin | Create |
| PUT | `/UpdateCategory` | Admin | Update |
| DELETE | `/RemoveCategory` | Admin | Delete |
| POST | `/AssignFeatureToCategory` | Admin | Assign feature |
| DELETE | `/RemoveFeatureFromCategory/...` | Admin | Remove feature |

### Orders / basket — `api/Order`

Checkout flow: client sends `ProductId` + `Quantity` → server enriches prices/stock → on register, stock is validated & deducted and an order is persisted.

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/basket` | Public | Enrich basket (server prices & availability) |
| GET | `/check-quantity` | Public | Check stock for one product |
| POST | `/register` | User | Place order (deduct stock) |
| GET | `/my-orders` | User | Current user's orders |
| GET | `/{orderId}` | User/Admin | Order details (own order, or any if admin) |
| POST | `/{orderId}/cancel` | User/Admin | Cancel & restore stock |

### Users & roles

| Controller | Base route | Auth | Notes |
|------------|------------|------|-------|
| UserManagement | `UserManagement` | User / Admin | `/me`, `/UpdateMyProfile` (User); `/getAll`, delete (Admin) |
| RoleManager | `RoleManager` | Admin | CRUD roles |
| UserRoles | `api/UserRoles` | Admin | Assign / remove roles |

## Sale flow (MVP)

```text
1. POST /api/Order/basket          → prices & stock from catalogue
2. POST /api/Order/register        → validate → deduct stock → save Order + OrderDetails
3. GET  /api/Order/my-orders       → list
4. POST /api/Order/{id}/cancel     → restore stock + status Cancelled
```

Prices always come from the server (never trusted from the client).

## Running Tests

```bash
dotnet test Application.Tests/Application.Tests.csproj
```

Tests cover Category, Product, Feature, and User application services with mocked dependencies (no database required).

## Project Structure

```text
Shopping/
├── Application/                             # Use cases
├── Application.DTOs/                        # DTOs & Identity models
├── ApplicationServiceContract/              # Application interfaces
├── Application.Tests/                       # Unit tests
├── Catalogue.Domain/                        # Catalogue entities
├── Catalogue.DomainServiceContract/         # Catalogue repo contracts
├── FramWork.Application/                    # Shared helpers / results
├── Framwork.Domain/                         # Generic repo base
├── Infrastracture.Catalogue.EF.Persistance/ # Catalogue EF + media storage
├── Sale.Domain/                             # Order entities
├── Sale.DomainServiceContract/              # Order repo contracts
├── Infrastructure.Sale/                     # Sale EF persistence
├── Security/                                # Identity + JWT
└── Shopping/                                # API host
    ├── Controllers/
    ├── Security/                            # Role constants + seeder
    ├── Program.cs
    └── appsettings.json
```

## Roadmap / not in MVP

- Payment gateway
- Shipping status workflow beyond Confirmed / Cancelled
- Discounts / coupons
- Default admin seed from configuration

## License

This project is provided as-is for learning and portfolio use. Add a license file (e.g. MIT) if you plan to open-source it formally.
