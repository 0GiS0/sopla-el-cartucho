# 🎮 Sopla el Cartucho 💨

<div align="center">

[![YouTube Channel Subscribers](https://img.shields.io/youtube/channel/subscribers/UC140iBrEZbOtvxWsJ-Tb0lQ?style=for-the-badge&logo=youtube&logoColor=white&color=red)](https://www.youtube.com/c/GiselaTorres?sub_confirmation=1)
[![GitHub followers](https://img.shields.io/github/followers/0GiS0?style=for-the-badge&logo=github&logoColor=white)](https://github.com/0GiS0)
[![LinkedIn Follow](https://img.shields.io/badge/LinkedIn-S%C3%ADgueme-blue?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/giselatorresbuitrago/)
[![X Follow](https://img.shields.io/badge/X-S%C3%ADgueme-black?style=for-the-badge&logo=x&logoColor=white)](https://twitter.com/0GiS0)

</div>

---

![CI](https://github.com/0GiS0/sopla-el-cartucho/actions/workflows/ci.yml/badge.svg)
![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-purple?style=flat-square)
![ASP.NET MVC](https://img.shields.io/badge/ASP.NET%20MVC-3-blue?style=flat-square)

¡Hola developer 👋🏻! En este repo encontrarás **Sopla el Cartucho**, una tienda online de videojuegos retro construida con .NET Framework 4.8 y ASP.NET MVC. Es un proyecto legacy intencional que usamos como ejemplo para workshops de migración a .NET 8+ y ASP.NET Core. Porque todos lo hemos hecho: soplar el cartucho para que funcione 💨

<a href="https://youtu.be/ONSog4WgUmw">
  <img src="https://img.youtube.com/vi/ONSog4WgUmw/maxresdefault.jpg" alt="Sopla el Cartucho - Tienda Retro con .NET Framework" width="100%" />
</a>

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
