# .NET 10 Migration Plan - SoplaElCartucho

## Table of Contents

- [Executive Summary](#executive-summary)
- [Migration Strategy](#migration-strategy)
- [Detailed Dependency Analysis](#detailed-dependency-analysis)
- [Project-by-Project Plans](#project-by-project-plans)
  - [Tier 1: SoplaElCartucho.Data](#tier-1-soplaelcartuchodata)
  - [Tier 2: SoplaElCartucho.Business](#tier-2-soplaelcartuchobusiness)
  - [Tier 3: SoplaElCartucho.Web](#tier-3-soplaelcartuchoweb)
  - [Tier 4: SoplaElCartucho.Tests](#tier-4-soplaelcartuchotests)
- [Risk Management](#risk-management)
- [Testing & Validation Strategy](#testing--validation-strategy)
- [Complexity & Effort Assessment](#complexity--effort-assessment)
- [Source Control Strategy](#source-control-strategy)
- [Success Criteria](#success-criteria)

---

## Executive Summary

### Scenario Description

Migration of **SoplaElCartucho** solution from **.NET Framework 4.8** to **.NET 10.0 (LTS)**. This is a retro videogame store web application built with ASP.NET MVC that requires migration to ASP.NET Core.

### Scope

| Metric | Value |
|--------|-------|
| **Total Projects** | 4 |
| **Total LOC** | 2,441 |
| **Estimated LOC to Modify** | 431+ (17.7%) |
| **NuGet Packages** | 6 (all compatible or included in framework) |
| **API Incompatibilities** | 431 (390 binary, 41 source) |

### Current State → Target State

| Project | Current | Target | Complexity |
|---------|---------|--------|------------|
| SoplaElCartucho.Data | net48 (Classic) | net10.0 (SDK-style) | 🟢 Low |
| SoplaElCartucho.Business | net48 (Classic) | net10.0 (SDK-style) | 🟢 Low |
| SoplaElCartucho.Web | net48 (Classic WAP) | net10.0 (SDK-style) | 🔴 High |
| SoplaElCartucho.Tests | net48 (Classic) | net10.0 (SDK-style) | 🟢 Low |

### Complexity Assessment

**Classification: Medium Complexity**

| Factor | Value | Assessment |
|--------|-------|------------|
| Project Count | 4 | ≤15 ✓ |
| Dependency Depth | 3 tiers | ≤4 ✓ |
| High-Risk Projects | 1 (Web) | ≤2 ✓ |
| Security Vulnerabilities | 0 | None ✓ |
| Circular Dependencies | None | Clean graph ✓ |

### Critical Issues

| Category | Count | Impact |
|----------|-------|--------|
| ASP.NET Framework APIs (System.Web) | 431 | High - Complete architectural migration required |
| SDK-style Conversion | 4 projects | Medium - All projects need conversion |
| Global.asax Migration | 1 | Medium - Migrate to Program.cs |
| Route Registration | 1 | Medium - Migrate to endpoint routing |

### Selected Strategy

**Bottom-Up (Dependency-First) Strategy** with phase-based execution:
- Migrate leaf nodes first, applications last
- Each tier validated before proceeding
- Maintains working solution at each checkpoint

### Iteration Strategy

- **Phase 1:** Foundation (skeleton, discovery, strategy) - 3 iterations ✓
- **Phase 2:** Dependency analysis, migration strategy, project stubs - 3 iterations
- **Phase 3:** Dynamic detail generation - 4 iterations (one per tier)
- **Expected Total:** ~10 iterations

---

## Migration Strategy

### Approach: Bottom-Up (Dependency-First)

**Selected over All-At-Once because:**
- Clear 4-tier dependency hierarchy exists
- One high-risk project (Web) benefits from stable foundation
- Allows incremental validation
- Maintains working solution at each checkpoint
- Lower risk per change

### Strategy Rationale

| Factor | Assessment | Impact on Strategy |
|--------|------------|-------------------|
| Project count (4) | Small | Could use all-at-once, but bottom-up preferred for risk isolation |
| High-risk project | Web (431 API issues) | Bottom-up ensures stable dependencies before tackling complexity |
| Dependency depth | 3 tiers | Clean hierarchy ideal for bottom-up |
| Test coverage | Exists (Tests project) | Validates each tier before proceeding |

### Execution Phases

#### Phase A: Tier 1 - Foundation Layer
- **Project:** SoplaElCartucho.Data
- **Goal:** Establish upgraded foundation
- **Validation:** Build succeeds, no API changes

#### Phase B: Tier 2 - Service Layer
- **Project:** SoplaElCartucho.Business
- **Goal:** Migrate services on stable data layer
- **Validation:** Build succeeds, service interfaces unchanged

#### Phase C: Tier 3 - Application Layer
- **Project:** SoplaElCartucho.Web
- **Goal:** Complete ASP.NET MVC → ASP.NET Core migration
- **Validation:** Application builds, basic navigation works

#### Phase D: Tier 4 - Validation Layer
- **Project:** SoplaElCartucho.Tests
- **Goal:** Ensure test infrastructure works on .NET 10
- **Validation:** All tests compile and pass

### Parallel vs Sequential Execution

**Sequential execution required:**
- Each tier must be fully validated before starting the next
- No parallel execution within tiers (single project per tier)
- Exception: Tests could theoretically start after Business, but kept sequential for simplicity

### Tier Completion Criteria

Before moving to the next tier:
1. ✅ Project converted to SDK-style
2. ✅ Target framework updated to net10.0
3. ✅ All package references resolved
4. ✅ Project builds without errors
5. ✅ Project builds without warnings (where possible)
6. ✅ Dependent projects (on old framework) still build

---

## Detailed Dependency Analysis

### Dependency Graph Visualization

```
Tier 4: [Tests]
         ↓
Tier 3: [Web] ─────────────┐
         ↓                 ↓
Tier 2: [Business] ────────┤
         ↓                 ↓
Tier 1: [Data] ←───────────┘
```

### Tier Breakdown

| Tier | Projects | Dependencies | Dependants | Rationale |
|------|----------|--------------|------------|-----------|
| **Tier 1** | Data | None (leaf) | Business, Web | Pure data layer, no project dependencies |
| **Tier 2** | Business | Data | Web, Tests | Service layer depending only on Tier 1 |
| **Tier 3** | Web | Data, Business | None | Application layer, depends on Tier 1 & 2 |
| **Tier 4** | Tests | Business | None | Test project, migrates last after tested projects |

### Project Dependency Details

#### SoplaElCartucho.Data (Tier 1 - Leaf Node)
- **Dependencies:** 0 internal projects
- **Dependants:** SoplaElCartucho.Business, SoplaElCartucho.Web
- **Migration Risk:** 🟢 Low - No dependencies to coordinate

#### SoplaElCartucho.Business (Tier 2)
- **Dependencies:** SoplaElCartucho.Data
- **Dependants:** SoplaElCartucho.Web, SoplaElCartucho.Tests
- **Migration Risk:** 🟢 Low - Single dependency, simple service classes

#### SoplaElCartucho.Web (Tier 3)
- **Dependencies:** SoplaElCartucho.Data, SoplaElCartucho.Business
- **Dependants:** None
- **Migration Risk:** 🔴 High - ASP.NET MVC to ASP.NET Core migration

#### SoplaElCartucho.Tests (Tier 4)
- **Dependencies:** SoplaElCartucho.Business
- **Dependants:** None
- **Migration Risk:** 🟢 Low - MSTest already compatible

### Critical Path

```
Data → Business → Web → Tests
  ↓
  └──────→ Web
```

The critical path goes through Data → Business → Web. Tests can be migrated in parallel with Web or after, but we place them in Tier 4 to ensure all production code is migrated first.

### Circular Dependency Analysis

**No circular dependencies detected.** The dependency graph is a clean DAG (Directed Acyclic Graph).

---

## Project-by-Project Plans

### Tier 1: SoplaElCartucho.Data

#### Project Info

| Attribute | Value |
|-----------|-------|
| **Path** | `SoplaElCartucho.Data\SoplaElCartucho.Data.csproj` |
| **Type** | ClassicClassLibrary |
| **LOC** | 81 |
| **Files** | 4 |
| **Complexity** | 🟢 Low |

#### Current State

- **Target Framework:** net48
- **SDK-style:** False (Classic .csproj)
- **Dependencies:** 0 internal projects
- **Dependants:** Business, Web
- **NuGet Packages:** None
- **API Incompatibilities:** 0

#### Target State

- **Target Framework:** net10.0
- **SDK-style:** True
- **Package References:** None required

#### Migration Steps

##### 1. Prerequisites
- Ensure .NET 10 SDK installed
- Verify branch is `upgrade-to-NET10`

##### 2. Project File Conversion
Convert from classic .csproj to SDK-style:

**Before (Classic):**
```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="15.0" ...>
  <PropertyGroup>
    <TargetFrameworkVersion>v4.8</TargetFrameworkVersion>
    ...
  </PropertyGroup>
  <ItemGroup>
    <Compile Include="..." />
    ...
  </ItemGroup>
</Project>
```

**After (SDK-style):**
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>disable</Nullable>
  </PropertyGroup>
</Project>
```

##### 3. Package Updates
No package updates required - project has no NuGet dependencies.

##### 4. Expected Breaking Changes
None - all 66 APIs analyzed are compatible.

##### 5. Code Modifications
None expected - pure library with compatible APIs.

##### 6. Testing Strategy
- Build verification
- Verify dependent projects (Business, Web) still build on net48

##### 7. Validation Checklist
- [ ] Project converted to SDK-style
- [ ] TargetFramework set to net10.0
- [ ] Build succeeds without errors
- [ ] Build succeeds without warnings
- [ ] SoplaElCartucho.Business (net48) still builds
- [ ] SoplaElCartucho.Web (net48) still builds

### Tier 2: SoplaElCartucho.Business

#### Project Info

| Attribute | Value |
|-----------|-------|
| **Path** | `SoplaElCartucho.Business\SoplaElCartucho.Business.csproj` |
| **Type** | ClassicClassLibrary |
| **LOC** | 93 |
| **Files** | 4 |
| **Complexity** | 🟢 Low |

#### Current State

- **Target Framework:** net48
- **SDK-style:** False (Classic .csproj)
- **Dependencies:** SoplaElCartucho.Data
- **Dependants:** Web, Tests
- **NuGet Packages:** None
- **API Incompatibilities:** 0

#### Target State

- **Target Framework:** net10.0
- **SDK-style:** True
- **Package References:** None required
- **Project References:** SoplaElCartucho.Data (net10.0)

#### Migration Steps

##### 1. Prerequisites
- ✅ Tier 1 (Data) must be completed and validated
- Data project must be on net10.0 and building successfully

##### 2. Project File Conversion
Convert from classic .csproj to SDK-style:

**After (SDK-style):**
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>disable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\SoplaElCartucho.Data\SoplaElCartucho.Data.csproj" />
  </ItemGroup>
</Project>
```

##### 3. Package Updates
No package updates required - project has no NuGet dependencies.

##### 4. Expected Breaking Changes
None - all 66 APIs analyzed are compatible.

##### 5. Code Modifications
None expected - service classes use standard .NET APIs.

##### 6. Testing Strategy
- Build verification
- Verify dependent projects (Web, Tests) still build on net48

##### 7. Validation Checklist
- [ ] Project converted to SDK-style
- [ ] TargetFramework set to net10.0
- [ ] ProjectReference to Data is correct
- [ ] Build succeeds without errors
- [ ] Build succeeds without warnings
- [ ] SoplaElCartucho.Web (net48) still builds
- [ ] SoplaElCartucho.Tests (net48) still builds

### Tier 3: SoplaElCartucho.Web

#### Project Info

| Attribute | Value |
|-----------|-------|
| **Path** | `SoplaElCartucho.Web\SoplaElCartucho.Web.csproj` |
| **Type** | WAP (Web Application Project) |
| **LOC** | 2,188 |
| **Files** | 49 |
| **Complexity** | 🔴 High |

#### Current State

- **Target Framework:** net48
- **SDK-style:** False (Classic WAP .csproj)
- **Dependencies:** SoplaElCartucho.Data, SoplaElCartucho.Business
- **Dependants:** None
- **NuGet Packages:** 4 (Microsoft.AspNet.Mvc, Razor, WebPages, Web.Infrastructure)
- **API Incompatibilities:** 431 (390 binary, 41 source)

#### Target State

- **Target Framework:** net10.0
- **SDK-style:** True (Microsoft.NET.Sdk.Web)
- **Package References:** None (ASP.NET Core included in SDK)
- **Project References:** Data (net10.0), Business (net10.0)

#### Migration Steps

##### 1. Prerequisites
- ✅ Tier 1 (Data) completed
- ✅ Tier 2 (Business) completed
- Both dependencies on net10.0 and building successfully

##### 2. Project File Conversion
Convert from WAP to SDK-style web project:

**After (SDK-style):**
```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>disable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\SoplaElCartucho.Data\SoplaElCartucho.Data.csproj" />
    <ProjectReference Include="..\SoplaElCartucho.Business\SoplaElCartucho.Business.csproj" />
  </ItemGroup>
</Project>
```

##### 3. Package Updates

| Package | Current | Action | Reason |
|---------|---------|--------|--------|
| Microsoft.AspNet.Mvc | 5.2.7 | Remove | Included in Microsoft.NET.Sdk.Web |
| Microsoft.AspNet.Razor | 3.2.7 | Remove | Included in Microsoft.NET.Sdk.Web |
| Microsoft.AspNet.WebPages | 3.2.7 | Remove | Included in Microsoft.NET.Sdk.Web |
| Microsoft.Web.Infrastructure | 1.0.0.0 | Remove | Included in Microsoft.NET.Sdk.Web |

##### 4. Expected Breaking Changes

**4.1 Controller Base Class Migration**

| Old (System.Web.Mvc) | New (Microsoft.AspNetCore.Mvc) |
|---------------------|-------------------------------|
| `System.Web.Mvc.Controller` | `Microsoft.AspNetCore.Mvc.Controller` |
| `System.Web.Mvc.ActionResult` | `Microsoft.AspNetCore.Mvc.IActionResult` |
| `System.Web.Mvc.ViewResult` | `Microsoft.AspNetCore.Mvc.ViewResult` |
| `System.Web.Mvc.JsonResult` | `Microsoft.AspNetCore.Mvc.JsonResult` |
| `System.Web.Mvc.RedirectToRouteResult` | `Microsoft.AspNetCore.Mvc.RedirectToActionResult` |

**4.2 Attributes Migration**

| Old | New | Count |
|-----|-----|-------|
| `[HttpPost]` | `[HttpPost]` | 11 (namespace change only) |
| `[HttpGet]` | `[HttpGet]` | 1 (namespace change only) |
| `[ValidateAntiForgeryToken]` | `[ValidateAntiForgeryToken]` | 6 (namespace change only) |
| `[Authorize]` | `[Authorize]` | 4 (namespace change only) |
| `[AllowAnonymous]` | `[AllowAnonymous]` | 5 (namespace change only) |

**4.3 Session Management (14 occurrences)**

| Old | New |
|-----|-----|
| `HttpSessionStateBase` | `ISession` (via `HttpContext.Session`) |
| `Session["key"]` | `HttpContext.Session.GetString("key")` / `SetString()` |

**4.4 ViewBag/TempData (43 + 21 occurrences)**

| Feature | Migration |
|---------|-----------|
| `ViewBag` | Same API, different namespace |
| `TempData` | Same API, requires `AddControllersWithViews()` |

**4.5 Authentication (3 occurrences)**

| Old | New |
|-----|-----|
| `FormsAuthentication.SetAuthCookie()` | Cookie Authentication middleware |
| `FormsAuthentication.SignOut()` | `HttpContext.SignOutAsync()` |

##### 5. Code Modifications

**5.1 Create Program.cs** (migrate from Global.asax.cs)

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Cuenta/Login";
    });

var app = builder.Build();

// Configure pipeline
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// Map routes (from RegisterRoutes)
app.MapControllerRoute(
    name: "CatalogoConsola",
    pattern: "catalogo/{consola}",
    defaults: new { controller = "Catalogo", action = "PorConsola" });

app.MapControllerRoute(
    name: "DetalleJuego",
    pattern: "juego/{id}",
    defaults: new { controller = "Catalogo", action = "Detalle" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

**5.2 Update Controller Namespaces**

Replace in all controllers:
```csharp
// Old
using System.Web.Mvc;
using System.Web;

// New
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
```

**5.3 Update Session Access**

```csharp
// Old
Session["Carrito"] = items;
var items = Session["Carrito"] as List<CarritoItem>;

// New  
HttpContext.Session.SetString("Carrito", JsonSerializer.Serialize(items));
var items = JsonSerializer.Deserialize<List<CarritoItem>>(
    HttpContext.Session.GetString("Carrito") ?? "[]");
```

**5.4 Update Authentication**

```csharp
// Old
FormsAuthentication.SetAuthCookie(usuario, false);

// New
var claims = new List<Claim> { new Claim(ClaimTypes.Name, usuario) };
var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
await HttpContext.SignInAsync(new ClaimsPrincipal(identity));
```

**5.5 Update Razor Views**

- Replace `@using System.Web.Mvc.Html` with `@using Microsoft.AspNetCore.Mvc.Rendering`
- Update `_ViewImports.cshtml` with ASP.NET Core tag helpers
- Update `@Html.AntiForgeryToken()` to `<form asp-antiforgery="true">` or keep as-is

**5.6 Files to Delete**

- `Global.asax`
- `Global.asax.cs`
- `Web.config` (replace with `appsettings.json` if needed)
- `packages.config`

**5.7 Files to Create**

- `Program.cs`
- `appsettings.json` (optional, for configuration)
- `Views/_ViewImports.cshtml` (update existing or create)

##### 6. Testing Strategy

| Test Type | Scope | Validation |
|-----------|-------|------------|
| Build | Project | Compiles without errors |
| Smoke | Application | Starts and serves home page |
| Navigation | Controllers | All routes accessible |
| Session | Carrito | Add/remove items works |
| Auth | Cuenta | Login/logout works |

##### 7. Validation Checklist

- [ ] Project converted to SDK-style (Microsoft.NET.Sdk.Web)
- [ ] TargetFramework set to net10.0
- [ ] All NuGet packages removed (included in SDK)
- [ ] Program.cs created with route configuration
- [ ] Global.asax removed
- [ ] All controller namespaces updated
- [ ] Session handling migrated
- [ ] Authentication migrated
- [ ] Build succeeds without errors
- [ ] Application starts successfully
- [ ] Home page loads
- [ ] Catalog navigation works
- [ ] Cart (Carrito) functions work
- [ ] Login/logout works

### Tier 4: SoplaElCartucho.Tests

#### Project Info

| Attribute | Value |
|-----------|-------|
| **Path** | `SoplaElCartucho.Tests\SoplaElCartucho.Tests.csproj` |
| **Type** | ClassicClassLibrary (Test) |
| **LOC** | 79 |
| **Files** | 3 |
| **Complexity** | 🟢 Low |

#### Current State

- **Target Framework:** net48
- **SDK-style:** False (Classic .csproj)
- **Dependencies:** SoplaElCartucho.Business
- **Dependants:** None
- **NuGet Packages:** MSTest.TestAdapter 3.1.1, MSTest.TestFramework 3.1.1
- **API Incompatibilities:** 0

#### Target State

- **Target Framework:** net10.0
- **SDK-style:** True
- **Package References:** MSTest.TestAdapter, MSTest.TestFramework (compatible)
- **Project References:** SoplaElCartucho.Business (net10.0)

#### Migration Steps

##### 1. Prerequisites
- ✅ Tier 1 (Data) completed
- ✅ Tier 2 (Business) completed
- ✅ Tier 3 (Web) completed
- Business project on net10.0 and building successfully

##### 2. Project File Conversion
Convert from classic .csproj to SDK-style:

**After (SDK-style):**
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>disable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.12.0" />
    <PackageReference Include="MSTest.TestAdapter" Version="3.1.1" />
    <PackageReference Include="MSTest.TestFramework" Version="3.1.1" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\SoplaElCartucho.Business\SoplaElCartucho.Business.csproj" />
  </ItemGroup>
</Project>
```

##### 3. Package Updates

| Package | Current | Target | Reason |
|---------|---------|--------|--------|
| MSTest.TestAdapter | 3.1.1 | 3.1.1 | ✅ Compatible |
| MSTest.TestFramework | 3.1.1 | 3.1.1 | ✅ Compatible |
| Microsoft.NET.Test.Sdk | (new) | 17.12.0 | Required for .NET test discovery |

##### 4. Expected Breaking Changes
None - MSTest APIs are compatible across .NET Framework and .NET.

##### 5. Code Modifications
None expected - test classes use standard MSTest attributes.

##### 6. Testing Strategy
- Build verification
- Run all tests via `dotnet test`
- Verify test discovery works

##### 7. Validation Checklist
- [ ] Project converted to SDK-style
- [ ] TargetFramework set to net10.0
- [ ] Microsoft.NET.Test.Sdk package added
- [ ] ProjectReference to Business is correct
- [ ] Build succeeds without errors
- [ ] All tests discovered
- [ ] All tests pass

---

## Risk Management

### High-Risk Changes

| Project | Risk Level | Description | Mitigation |
|---------|------------|-------------|------------|
| **Web** | 🔴 High | 431 API incompatibilities, ASP.NET MVC → Core | Bottom-up ensures stable deps first; incremental controller migration |
| **Web** | 🟡 Medium | Global.asax → Program.cs migration | Follow documented patterns; preserve existing route structure |
| **Web** | 🟡 Medium | Session management changes | Migrate to ASP.NET Core session with distributed cache option |
| **Web** | 🟡 Medium | FormsAuthentication → Cookie Authentication | Standard migration pattern, well-documented |

### Security Vulnerabilities

**None identified.** All NuGet packages are either compatible or superseded by framework references.

### Contingency Plans

| Risk | Contingency |
|------|-------------|
| **Breaking changes in controllers** | Use `Microsoft.AspNetCore.Mvc.ViewFeatures` for ViewBag/TempData compatibility |
| **Session state issues** | Implement `IDistributedCache` with in-memory provider initially |
| **Authentication breaks** | Keep FormsAuthentication parallel until Cookie Auth verified |
| **Build failures cascade** | Git commit after each tier; rollback point available |

---

## Complexity & Effort Assessment

### Per-Project Complexity

| Project | Complexity | LOC | Files | Dependencies | API Issues | Risk Factors |
|---------|------------|-----|-------|--------------|------------|--------------|
| Data | 🟢 Low | 81 | 4 | 0 | 0 | None - pure conversion |
| Business | 🟢 Low | 93 | 4 | 1 | 0 | None - pure conversion |
| Web | 🔴 High | 2,188 | 49 | 2 | 431 | ASP.NET migration, session, auth |
| Tests | 🟢 Low | 79 | 3 | 1 | 0 | MSTest already compatible |

### Phase Complexity Assessment

| Phase | Tier | Projects | Complexity | Description |
|-------|------|----------|------------|-------------|
| A | 1 | Data | 🟢 Low | SDK conversion + TFM only |
| B | 2 | Business | 🟢 Low | SDK conversion + TFM only |
| C | 3 | Web | 🔴 High | Full ASP.NET Core migration |
| D | 4 | Tests | 🟢 Low | SDK conversion + TFM only |

### Resource Requirements

| Skill | Required For | Level |
|-------|--------------|-------|
| .NET SDK-style projects | All tiers | Basic |
| ASP.NET Core MVC | Tier 3 (Web) | Intermediate |
| ASP.NET Core Authentication | Tier 3 (Web) | Intermediate |
| MSTest on .NET Core | Tier 4 (Tests) | Basic |

---

## Testing & Validation Strategy

### Multi-Level Testing Approach

#### Per-Tier Testing

| Tier | Project | Tests |
|------|---------|-------|
| 1 | Data | Build verification, dependent projects still compile |
| 2 | Business | Build verification, dependent projects still compile |
| 3 | Web | Build, startup, smoke tests, functional validation |
| 4 | Tests | Build, test discovery, test execution |

#### Phase Validation Gates

**After Tier 1 (Data):**
- [ ] Data project builds on net10.0
- [ ] Business project (net48) still builds with Data reference
- [ ] Web project (net48) still builds with Data reference

**After Tier 2 (Business):**
- [ ] Business project builds on net10.0
- [ ] Web project (net48) still builds with Business reference
- [ ] Tests project (net48) still builds with Business reference

**After Tier 3 (Web):**
- [ ] Web project builds on net10.0
- [ ] Application starts without errors
- [ ] Home page renders correctly
- [ ] Navigation works (catalog, cart, account)
- [ ] Session persists across requests
- [ ] Authentication flow works

**After Tier 4 (Tests):**
- [ ] Tests project builds on net10.0
- [ ] All tests discovered by test runner
- [ ] All tests pass

### Full Solution Validation

After all tiers complete:
- [ ] `dotnet build SoplaElCartucho.sln` succeeds
- [ ] `dotnet test SoplaElCartucho.sln` passes all tests
- [ ] Application runs end-to-end
- [ ] No security vulnerabilities (none identified)
- [ ] No package dependency conflicts

---

## Complexity & Effort Assessment

[To be filled]

---

## Source Control Strategy

### Branch Strategy

| Branch | Purpose |
|--------|---------|
| `master` | Source branch (stable, pre-upgrade) |
| `upgrade-to-NET10` | Upgrade work branch (current) |

### Commit Strategy

**Commit after each tier completion:**

| Tier | Commit Message |
|------|----------------|
| 1 | `🔄 Upgrade SoplaElCartucho.Data to .NET 10` |
| 2 | `🔄 Upgrade SoplaElCartucho.Business to .NET 10` |
| 3 | `🔄 Upgrade SoplaElCartucho.Web to .NET 10` |
| 4 | `🔄 Upgrade SoplaElCartucho.Tests to .NET 10` |
| Final | `✅ Complete .NET 10 upgrade` |

### Rollback Points

Each tier commit serves as a rollback point:
- If Tier 2 fails: `git reset --hard <tier-1-commit>`
- If Tier 3 fails: `git reset --hard <tier-2-commit>`
- If Tier 4 fails: `git reset --hard <tier-3-commit>`

### Pull Request Strategy

After all tiers complete:
1. Push `upgrade-to-NET10` branch
2. Create PR: `🚀 Upgrade solution to .NET 10`
3. Include summary of changes per tier
4. Request review
5. Squash merge to `master`

---

## Success Criteria

### Technical Criteria

- [ ] All 4 projects target net10.0
- [ ] All 4 projects use SDK-style .csproj
- [ ] All builds succeed without errors
- [ ] All builds succeed without warnings (where possible)
- [ ] All tests pass
- [ ] No package dependency conflicts
- [ ] No security vulnerabilities

### Functional Criteria

- [ ] Application starts and serves requests
- [ ] Home page renders correctly
- [ ] Catalog browsing works (por consola, detalle)
- [ ] Shopping cart (carrito) works (add, remove, view)
- [ ] User authentication works (login, logout)
- [ ] Session state persists correctly

### Quality Criteria

- [ ] Code follows existing patterns (Spanish naming, ViewBag usage)
- [ ] Anti-pattern markers preserved (ANTI-PATRON, LEGACY comments)
- [ ] Test coverage maintained
- [ ] No regressions in functionality

### Process Criteria

- [ ] Bottom-up strategy followed (Data → Business → Web → Tests)
- [ ] Each tier validated before proceeding
- [ ] Commits made after each tier
- [ ] Source control strategy followed

---

## Appendix: API Migration Quick Reference

### Namespace Changes

| Old Namespace | New Namespace |
|---------------|---------------|
| `System.Web.Mvc` | `Microsoft.AspNetCore.Mvc` |
| `System.Web.Routing` | `Microsoft.AspNetCore.Routing` |
| `System.Web.Security` | `Microsoft.AspNetCore.Authentication` |
| `System.Web.SessionState` | `Microsoft.AspNetCore.Http` |

### Common Type Mappings

| Old Type | New Type |
|----------|----------|
| `Controller` | `Controller` (different namespace) |
| `ActionResult` | `IActionResult` |
| `HttpSessionStateBase` | `ISession` |
| `FormsAuthentication` | Cookie Authentication middleware |
| `RouteCollection` | Endpoint routing (`MapControllerRoute`) |

### Files Removed vs Created

| Removed | Created |
|---------|---------|
| `Global.asax` | `Program.cs` |
| `Global.asax.cs` | `appsettings.json` (optional) |
| `Web.config` | - |
| `packages.config` | - |
