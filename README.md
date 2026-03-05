# 🎮 Sopla el Cartucho 💨

> **Tu tienda de videojuegos retro favorita**

[![CI](https://github.com/0GiS0/sopla-el-cartucho/actions/workflows/ci.yml/badge.svg)](https://github.com/0GiS0/sopla-el-cartucho/actions/workflows/ci.yml)
![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-purple?style=flat-square)
![ASP.NET MVC](https://img.shields.io/badge/ASP.NET%20MVC-3-blue?style=flat-square)

## 📋 Descripción

**Sopla el Cartucho** es una tienda online de videojuegos retro construida con .NET Framework y ASP.NET MVC.

### ¿Por qué "Sopla el Cartucho"?

Porque todos lo hemos hecho: soplar el cartucho para que funcione. Es el ritual universal de todo gamer retro. 💨

## 🛠️ Stack Tecnológico

| Tecnología | Versión |
|------------|---------|
| .NET Framework | 4.8 |
| ASP.NET MVC | 3.0 |
| jQuery | 1.6.4 |
| ASP.NET Membership | - |

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
│   └── Repositories/              # Patrón Repository
├── SoplaElCartucho.Business/      # Lógica de negocio
│   └── Services/                  # Servicios
├── SoplaElCartucho.Tests/         # Tests (MSTest)
└── Database/
    └── Scripts/                   # Scripts SQL
```

## 🎮 Funcionalidades

- **Catálogo de juegos**: NES, SNES, Mega Drive, PlayStation, N64, Game Boy
- **Carrito de compras**: Gestión con Session State
- **Pedidos**: CRUD completo
- **Autenticación**: Login/Registro
- **Estética retro**: CSS 8-bit con efectos CRT

## 🔧 Requisitos

- Visual Studio 2019+
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

## 📄 Licencia

MIT

---

<div align="center">

**🎮 SOPLA EL CARTUCHO 💨**

*Hecho con nostalgia*

</div>
