# Sopla el Cartucho - Copilot Instructions

## Panorama general

- Aplicación web legacy de tienda retro en **ASP.NET MVC 5 / .NET Framework 4.8**. No uses `dotnet build`; trabaja con `msbuild`, Visual Studio y proyectos no SDK.
- Todo el dominio y los textos están en **español**. Mantén nombres como `Juego`, `Consola`, `Pedido`, `ObtenerTodos`, `CambiarEstado`.
- La solución real tiene 5 proyectos: `SoplaElCartucho.Web`, `SoplaElCartucho.Business`, `SoplaElCartucho.Data`, `SoplaElCartucho.Common`, `SoplaElCartucho.Tests`.

## Arquitectura y flujo de datos

- Flujo principal: `Web -> Business -> Data -> Common`.
- Los modelos compartidos viven en `SoplaElCartucho.Common/Models` (`Juego`, `Consola`, `CarritoItem`, `Pedido`, `DetallePedido`). No crees duplicados en `Web/Models`; esa carpeta existe pero no contiene el dominio real.
- Los controladores instancian servicios directamente (`new CatalogoService()`, `new PedidoService()`). Los servicios instancian repositorios directamente. **No introduzcas DI ni interfaces** salvo petición explícita.
- Ejemplos de límite entre capas: `CatalogoController` filtra y pagina datos del servicio; `PedidoService` delega en `PedidoRepository`; `JuegoRepository` usa `SqlCommand` manual.
- `CarritoService` es un stub casi vacío en `Business/Services/CarritoService.cs`; el carrito funcional vive en `SoplaElCartucho.Web/Controllers/CarritoController.cs` usando `Session`.

## Patrones legacy que deben preservarse

- Este repositorio es material de migración: conserva comentarios y estilo `ANTI-PATRÓN`, `LEGACY` y `MIGRACIÓN` cuando toques clases que ya los usan.
- Sigue el estilo actual: controladores y servicios concretos, `ViewBag`/`ViewData` en lugar de view models fuertes, `FormsAuthentication` en lugar de Identity.
- No modernices por iniciativa propia: evita `async`, DI, EF Core, record types o features ajenas a .NET Framework 4.8 si la tarea no lo pide.

## Web y comportamiento funcional

- Las rutas se registran en `SoplaElCartucho.Web/Global.asax.cs`: `CatalogoConsola`, `DetalleJuego`, `Default`.
- `HomeController` y `CatalogoController` leen catálogo desde `CatalogoService`; `PedidosController` arma `Pedido` desde `Session["Carrito"]`; `AccountController` valida login con contraseñas hardcodeadas (`retro123` y `admin`).
- El carrito guarda `List<CarritoItem>` en `Session`; `CarritoItem` y `Pedido` están marcados como `[Serializable]`.
- Constantes de negocio actuales: IVA `21%`, envío gratis `> 50`, gastos `4.99`, máximo `3` unidades por juego, máximo `10` ítems distintos, paginación de catálogo `8`.

## Datos e integraciones

- La capa `Data` usa **ADO.NET directo** con `System.Data.SqlClient`; consulta tablas `Juegos`, `Consolas`, `Pedidos`, `DetallesPedido`.
- La conexión sale de `SoplaElCartucho.Web/Web.config` y `SoplaElCartucho.Tests/App.config` con la clave `SoplaElCartuchoDb`, apuntando por defecto a `.`\`SQLEXPRESS`.
- La app inicializa la base al arrancar mediante `DatabaseInitializer.Inicializar()` en `Global.asax.cs` y ejecuta `SoplaElCartucho.Data/Scripts/CrearBaseDatos.sql` si faltan BD o tablas.
- `SoplaElCartucho.Data.csproj` aún referencia `System.Data.Entity`, pero el acceso activo del código inspeccionado es ADO.NET manual.

## Build, tests y flujo de trabajo

- Restauración y build esperados: `nuget restore SoplaElCartucho.sln` y luego `msbuild SoplaElCartucho.sln /p:Configuration=Release /p:Platform="Any CPU"`.
- Los tests usan **MSTest** (`MSTest.TestFramework` 3.1.1). Ejecuta desde Test Explorer o con `vstest.console.exe` como en `.github/workflows/ci.yml`.
- `CatalogoServiceTests` son de integración ligera y requieren SQL Server accesible; no asumas que todos los tests son unitarios puros.
- Cuando implementes trabajo derivado de issues, sigue `AGENTS.md`: usa **GitHub Flow** desde `master`.
