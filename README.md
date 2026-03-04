# 🎮 Sopla el Cartucho 💨

> **Tu tienda de videojuegos retro favorita** - Aplicación Legacy .NET Framework 3.5

![.NET Framework](https://img.shields.io/badge/.NET%20Framework-3.5-purple?style=flat-square)
![ASP.NET MVC](https://img.shields.io/badge/ASP.NET%20MVC-3-blue?style=flat-square)
![Status](https://img.shields.io/badge/Status-Legacy%20Demo-orange?style=flat-square)

## 📋 Descripción

**Sopla el Cartucho** es una aplicación web de demostración que simula una tienda online de videojuegos retro. Está construida deliberadamente con tecnologías **legacy** de .NET Framework para servir como caso de estudio en talleres de **migración a .NET moderno**.

### ¿Por qué "Sopla el Cartucho"?

Porque todos lo hemos hecho: soplar el cartucho para que funcione. Es el ritual universal de todo gamer retro. 💨

## 🎯 Objetivo

Esta aplicación está diseñada para:

1. **Demostrar una aplicación legacy típica** de empresas que aún usan .NET Framework
2. **Identificar anti-patrones comunes** en aplicaciones antiguas
3. **Practicar la migración** a .NET 8+ y ASP.NET Core
4. **Comparar** patrones de desarrollo antiguos vs modernos

## 🛠️ Stack Tecnológico (Legacy)

| Tecnología | Versión | Estado |
|------------|---------|--------|
| .NET Framework | 3.5 | ⚠️ Obsoleto |
| ASP.NET MVC | 3.0 | ⚠️ Obsoleto |
| Entity Framework | 4.0 (EDMX) | ⚠️ Obsoleto |
| jQuery | 1.6.4 | ⚠️ Muy antigua |
| ASP.NET Membership | - | ⚠️ Obsoleto desde 2016 |

## 📁 Estructura del Proyecto

```
SoplaElCartucho/
├── SoplaElCartucho.Web/           # Proyecto MVC principal
│   ├── Controllers/               # Controladores MVC
│   ├── Views/                     # Vistas Razor
│   ├── Models/                    # Modelos de dominio
│   ├── Content/                   # CSS, imágenes
│   ├── Scripts/                   # JavaScript (jQuery)
│   ├── Global.asax               # Configuración de aplicación
│   └── Web.config                # Configuración XML
├── SoplaElCartucho.Data/          # Capa de datos
│   ├── Model.edmx                 # Entity Framework EDMX
│   ├── DataSets/                  # DataSets tipados
│   └── Repositories/              # Patrón Repository
├── SoplaElCartucho.Business/      # Lógica de negocio
│   └── Services/                  # Servicios
├── SoplaElCartucho.Tests/         # Tests (MSTest)
└── Database/
    └── Scripts/                   # Scripts SQL
```

## 🚨 Anti-patrones Identificados

El código está **intencionalmente** lleno de anti-patrones para fines educativos:

### Arquitectura
- [ ] `Global.asax` monolítico
- [ ] Sin inyección de dependencias
- [ ] Controladores "gordos" con lógica de negocio
- [ ] Servicios sin interfaces

### Acceso a Datos
- [ ] Mezcla de EF EDMX + DataSets + ADO.NET
- [ ] Connection strings en texto plano
- [ ] Sin Unit of Work ni transacciones

### Seguridad
- [ ] ASP.NET Membership Provider (obsoleto)
- [ ] Algunas acciones sin `[ValidateAntiForgeryToken]`
- [ ] Session State InProc (no escala)

### Frontend
- [ ] jQuery 1.6.4 (muy antigua)
- [ ] Tablas HTML para layout
- [ ] Estilos inline
- [ ] Sin bundling/minificación

### Vistas
- [ ] Uso excesivo de ViewBag/ViewData
- [ ] Lógica de presentación en vistas
- [ ] Sin ViewModels fuertemente tipados

## 🚀 Guía de Migración

### Fase 1: Preparación
1. Actualizar a .NET Framework 4.8 (paso intermedio)
2. Instalar .NET Upgrade Assistant
3. Crear rama de migración

### Fase 2: Migración Core
1. Convertir proyectos a SDK-style
2. Migrar a .NET 8
3. Reemplazar `Web.config` por `appsettings.json`
4. Convertir `Global.asax` a `Program.cs`

### Fase 3: Modernización
1. Reemplazar Membership por ASP.NET Core Identity
2. Migrar EF EDMX a EF Core Code First
3. Implementar inyección de dependencias
4. Crear ViewModels fuertemente tipados

### Fase 4: Frontend
1. Actualizar jQuery o migrar a vanilla JS
2. Implementar bundling con webpack/vite
3. Reemplazar tablas por CSS Grid/Flexbox
4. Añadir TypeScript (opcional)

## 🎮 Funcionalidades

- **Catálogo de juegos**: NES, SNES, Mega Drive, PlayStation, N64, Game Boy
- **Carrito de compras**: Gestión con Session State
- **Pedidos**: CRUD completo
- **Autenticación**: Login/Registro con Membership
- **Estética retro**: CSS 8-bit con efectos CRT

## 🔧 Requisitos

- Visual Studio 2010+ (para .NET Framework 3.5)
- SQL Server Express / LocalDB
- IIS Express

## 📦 Instalación

1. Clonar el repositorio
2. Ejecutar script SQL: `Database/Scripts/001_CreateDatabase.sql`
3. Actualizar connection string en `Web.config`
4. Compilar y ejecutar

## 🐣 Easter Eggs

- **Código Konami**: ↑ ↑ ↓ ↓ ← → ← → B A
- **Click secreto**: 5 clicks en el logo
- **Consola del navegador**: Mensajes especiales en F12

## 📚 Recursos de Migración

- [.NET Upgrade Assistant](https://docs.microsoft.com/en-us/dotnet/core/porting/upgrade-assistant-overview)
- [Migrating from ASP.NET MVC to ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/migration/mvc)
- [EF6 to EF Core](https://docs.microsoft.com/en-us/ef/efcore-and-ef6/)

## 📄 Licencia

MIT - Uso libre para fines educativos.

---

<div align="center">

**🎮 SOPLA EL CARTUCHO 💨**

*Hecho con nostalgia y código legacy*

⚠️ **ADVERTENCIA**: Esta aplicación es SOLO para demostración.  
NO usar en producción.

</div>
