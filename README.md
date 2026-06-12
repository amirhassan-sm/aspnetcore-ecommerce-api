# Shopping

A layered ASP.NET Core project built with Clean Architecture principles.

## Overview

`Shopping` is a sample store management/backend project designed with a clear separation of concerns using a layered architecture.  
The solution is organized around core business logic, application services, infrastructure, and the API host.

This structure makes the project easier to maintain, test, and extend as new features such as products, categories, security, and validations are added.

## Architecture

The solution follows a Clean Architecture / Layered Architecture approach:

- `Catalogue.Domain`  
  Contains core domain models and business rules related to catalogue features.

- `Catalogue.DomainServiceContract`  
  Defines domain service contracts and abstractions.

- `Application`  
  Contains application logic, use cases, and orchestration between layers.

- `Application.DTOs`  
  Includes Data Transfer Objects used between layers and for API communication.

- `ApplicationServiceContract`  
  Contains application-level service contracts.

- `Infrastructure.Catalogue.EF.Persistance`  
  Implements persistence logic using Entity Framework Core, including repositories, configurations, and migrations.

- `Security`  
  Contains security-related implementation details.

- `Security.DomainServiceContract`  
  Defines security-related contracts and abstractions.

- `Shopping`  
  The main ASP.NET Core Web API host project.

## Project Structure
```text
Shopping
|
|-- Application
|-- Application.DTOs
|-- ApplicationServiceContract
|-- Catalogue.Domain
|-- Catalogue.DomainServiceContract
|-- FramWork.Application
|-- Framwork.Domain
|-- Infrastructure.Catalogue.EF.Persistance
|-- Security
|-- Security.DomainServiceContract
`-- Shopping
