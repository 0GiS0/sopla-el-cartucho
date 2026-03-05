# SoplaElCartucho .NET 10.0 Upgrade Tasks

## Overview

This document tracks the bottom-up migration of SoplaElCartucho solution from .NET Framework 4.8 to .NET 10.0. Projects will be upgraded tier-by-tier starting from leaf dependencies (Data) and progressing to applications (Web) and tests.

**Progress**: 8/8 tasks complete (100%) ![0%](https://progress-bar.xyz/100)

---

## Tasks

### [✓] TASK-001: Verify prerequisites *(Completed: 2026-03-05 09:59)*
**References**: Plan §Prerequisites

- [✓] (1) Verify .NET 10 SDK installed per Plan §Prerequisites
- [✓] (2) .NET 10 SDK available (**Verify**)

---

### [✓] TASK-002: Upgrade Tier 1 (Data) *(Completed: 2026-03-05 10:20)*
**References**: Plan §Tier 1: SoplaElCartucho.Data

- [✓] (1) Convert SoplaElCartucho.Data to SDK-style project per Plan §Tier 1 §2
- [✓] (2) Update TargetFramework to net10.0
- [✓] (3) Project file converted and TFM updated (**Verify**)
- [✓] (4) Build SoplaElCartucho.Data project
- [✓] (5) Build succeeds with 0 errors (**Verify**)
- [✓] (6) Commit changes with message: "🔄 Upgrade SoplaElCartucho.Data to .NET 10"

---

### [✓] TASK-003: Upgrade Tier 2 (Business) *(Completed: 2026-03-05 10:22)*
**References**: Plan §Tier 2: SoplaElCartucho.Business

- [✓] (1) Convert SoplaElCartucho.Business to SDK-style project per Plan §Tier 2 §2
- [✓] (2) Update TargetFramework to net10.0
- [✓] (3) Update ProjectReference to SoplaElCartucho.Data
- [✓] (4) Project file converted and references updated (**Verify**)
- [✓] (5) Build SoplaElCartucho.Business project
- [✓] (6) Build succeeds with 0 errors (**Verify**)
- [✓] (7) Commit changes with message: "🔄 Upgrade SoplaElCartucho.Business to .NET 10"

---

### [✓] TASK-004: Update Tier 3 (Web) project structure *(Completed: 2026-03-05 10:23)*
**References**: Plan §Tier 3: SoplaElCartucho.Web §2, §3

- [✓] (1) Convert SoplaElCartucho.Web to SDK-style web project (Microsoft.NET.Sdk.Web) per Plan §Tier 3 §2
- [✓] (2) Update TargetFramework to net10.0
- [✓] (3) Remove ASP.NET MVC NuGet packages per Plan §Tier 3 §3 (included in SDK)
- [✓] (4) Update ProjectReferences to Data and Business projects
- [✓] (5) Project file converted and packages removed (**Verify**)

---

### [✓] TASK-005: Migrate ASP.NET Core infrastructure *(Completed: 2026-03-05 10:24)*
**References**: Plan §Tier 3 §5.1, §5.6, §5.7

- [✓] (1) Create Program.cs with startup configuration per Plan §Tier 3 §5.1 (services, middleware, routes)
- [✓] (2) Delete Global.asax and Global.asax.cs per Plan §Tier 3 §5.6
- [✓] (3) Delete Web.config and packages.config per Plan §Tier 3 §5.6
- [✓] (4) Program.cs created and old files removed (**Verify**)

---

### [✓] TASK-006: Update controllers and views *(Completed: 2026-03-05 10:29)*
**References**: Plan §Tier 3 §5.2, §5.3, §5.4, §5.5

- [✓] (1) Update controller namespaces per Plan §Tier 3 §5.2 (Microsoft.AspNetCore.Mvc)
- [✓] (2) Update session handling per Plan §Tier 3 §5.3 (ISession with JSON serialization)
- [✓] (3) Update authentication per Plan §Tier 3 §5.4 (Cookie Authentication)
- [✓] (4) Update Razor views namespaces per Plan §Tier 3 §5.5
- [✓] (5) All namespace updates applied (**Verify**)

---

### [✓] TASK-007: Build and validate Tier 3 (Web) *(Completed: 2026-03-05 10:32)*
**References**: Plan §Tier 3 §4, §6

- [✓] (1) Build SoplaElCartucho.Web project to identify errors
- [✓] (2) Fix all compilation errors found (reference Plan §Tier 3 §4 Breaking Changes for guidance)
- [✓] (3) Rebuild project after fixes
- [✓] (4) Build succeeds with 0 errors (**Verify**)
- [✓] (5) Commit changes with message: "🔄 Upgrade SoplaElCartucho.Web to .NET 10"

---

### [✓] TASK-008: Upgrade Tier 4 (Tests) *(Completed: 2026-03-05 10:36)*
**References**: Plan §Tier 4: SoplaElCartucho.Tests

- [✓] (1) Convert SoplaElCartucho.Tests to SDK-style project per Plan §Tier 4 §2
- [✓] (2) Update TargetFramework to net10.0
- [✓] (3) Add Microsoft.NET.Test.Sdk package (version 17.12.0) per Plan §Tier 4 §3
- [✓] (4) Update ProjectReference to SoplaElCartucho.Business
- [✓] (5) Project file converted and packages updated (**Verify**)
- [✓] (6) Build SoplaElCartucho.Tests project
- [✓] (7) Build succeeds with 0 errors (**Verify**)
- [✓] (8) Run all tests in SoplaElCartucho.Tests project
- [✓] (9) All tests pass with 0 failures (**Verify**)
- [✓] (10) Commit changes with message: "✅ Complete .NET 10 upgrade - SoplaElCartucho.Tests migrated"

---















