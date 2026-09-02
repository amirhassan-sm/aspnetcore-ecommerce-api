FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 10000
RUN mkdir -p /app/wwwroot/images/products \
    && chown -R app:app /app/wwwroot
USER app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Shopping/Shopping.csproj Shopping/
COPY Application/Application.csproj Application/
COPY ApplicationServiceContract/Application.Contrast.csproj ApplicationServiceContract/
COPY Application.DTOs/Application.DTOs.csproj Application.DTOs/
COPY Infrastracture.Catalogue.EF.Persistance/Infrastracture.Catalogue.EF.Persistance.csproj Infrastracture.Catalogue.EF.Persistance/
COPY Infrastructure.Sale/Infrastructure.Sale.csproj Infrastructure.Sale/
COPY Security/Infrastracture.Security.csproj Security/
COPY Catalogue.Domain/Catalogue.Domain.csproj Catalogue.Domain/
COPY Catalogue.DomainServiceContract/Catalogue.DomainServiceContract.csproj Catalogue.DomainServiceContract/
COPY Sale.Domain/Sale.Domain.csproj Sale.Domain/
COPY Sale.DomainServiceContract/Sale.DomainServiceContract.csproj Sale.DomainServiceContract/
COPY FramWork.Application/Application.Common.csproj FramWork.Application/
COPY Framwork.Domain/Framwork.Domain.csproj Framwork.Domain/

RUN dotnet restore Shopping/Shopping.csproj

COPY . .
WORKDIR /src/Shopping
RUN dotnet publish Shopping.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:10000
ENV PORT=10000
ENTRYPOINT ["dotnet", "Shopping.dll"]
