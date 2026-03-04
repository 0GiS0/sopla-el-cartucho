# Sopla el Cartucho - Copilot Instructions

## Project Overview

Retro videogame store web app. **Intentionally legacy** codebase used as a migration workshop target (to .NET 8+ / ASP.NET Core). All code is in **Spanish** (models, routes, comments, UI strings).

- **Framework:** .NET Framework 4.8, ASP.NET MVC (classic `System.Web.Mvc`), non-SDK-style `.csproj`
- **Language:** C# (no modern C# features beyond what .NET Framework 4.8 supports)
- **Build:** `MSBuild` / Visual Studio. No `dotnet build` support

## Solution Structure

```
SoplaElCartucho.Web/        ASP.NET MVC controllers, Razor views, domain models
SoplaElCartucho.Business/   Service classes (CatalogoService, CarritoService, PedidoService)
SoplaElCartucho.Data/       Repository classes (JuegoRepository, ConsolaRepository, PedidoRepository)
SoplaElCartucho.Tests/      MSTest unit tests
```

Layering: **Web -> Business -> Data**. Controllers should call services; services call repositories. Currently some controllers bypass services and use static in-memory data directly. This is an intentional anti-pattern.

## Key Conventions

- **Naming is in Spanish:** `Juego`, `Consola`, `Pedido`, `Carrito`, `ObtenerTodos()`, `AgregarItem()`, etc. Keep all new code in Spanish.
- **No dependency injection:** Services `new` up repositories in constructors. Controllers `new` up services or use static data. Do not introduce a DI container.
- **No interfaces on services/repositories.** They are concrete classes without abstractions.
- **Models live in `SoplaElCartucho.Web.Models`** (`Juego`, `Consola`, `CarritoItem`, `Pedido`, `DetallePedido`). Data-layer repositories return `object` (stub implementations).
- **Session-based cart:** `CarritoController` stores `List<CarritoItem>` in `HttpSessionState`. `CarritoItem` is `[Serializable]`.
- **ViewBag/ViewData** are used heavily to pass data to views. No strongly-typed ViewModels exist.
- **Routes** are registered in `Global.asax.cs` via `RegisterRoutes()`. Named routes: `CatalogoConsola`, `DetalleJuego`, `Default`.
- **Auth** uses `FormsAuthentication` + a hardcoded demo validator (password `retro123`).

## Anti-Pattern Awareness

The codebase contains **deliberate anti-patterns** annotated with `ANTI-PATRON` and `LEGACY` comments. When fixing bugs or adding features:

- **Preserve the anti-pattern style** unless the task explicitly asks to refactor or migrate.
- **Keep the comment markers** (`ANTI-PATRON`, `MIGRACION`). They are part of the educational material.
- Match the existing comment style: block comment header at the top of each class listing anti-patterns and migration notes.

## Testing

- **Framework:** MSTest (`[TestClass]`, `[TestMethod]`, `[TestInitialize]`)
- **Project:** `SoplaElCartucho.Tests` mirrors service classes (`CarritoServiceTests`, `CatalogoServiceTests`)
- **No mocking framework.** Tests use real service instances with stub repositories.
- **Run tests** via Visual Studio Test Explorer or `vstest.console.exe`

## Branching Workflow

Follow **GitHub Flow** when implementing new features from issues (per `AGENTS.md`): create a feature branch from `master`, implement, open a pull request.

## Business Constants (hardcoded)

| Constant | Value | Location |
|----------|-------|----------|
| IVA (tax) | 21% | `CarritoController` |
| Free shipping threshold | > 50 EUR | `CarritoController` |
| Shipping cost | 4.99 EUR | `CarritoController` |
| Max units per game | 3 | `CarritoController.Agregar` |
| Max distinct items in cart | 10 | `CarritoController.Agregar` |
| Catalog page size | 8 | `CatalogoController.ITEMS_POR_PAGINA` |
