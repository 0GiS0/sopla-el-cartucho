-- ============================================
-- 🎮 SOPLA EL CARTUCHO - Script de Creación de Base de Datos
-- ============================================
-- ⚠️ ANTI-PATRÓN LEGACY: Script SQL manual sin migraciones
-- 📝 MIGRACIÓN: Usar Entity Framework Core Migrations
-- ============================================

-- Tabla Consolas
CREATE TABLE Consolas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    ImagenUrl NVARCHAR(500),
    Fabricante NVARCHAR(100),
    AnioLanzamiento INT,
    Descripcion NVARCHAR(MAX),
    Orden INT DEFAULT 0,
    Activa BIT DEFAULT 1
);

-- Tabla Juegos
CREATE TABLE Juegos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(200) NOT NULL,
    Descripcion NVARCHAR(MAX),
    Precio DECIMAL(10,2) NOT NULL,
    Stock INT DEFAULT 0,
    ImagenUrl NVARCHAR(500),
    ConsolaId INT FOREIGN KEY REFERENCES Consolas(Id),
    Genero NVARCHAR(50),
    AnioLanzamiento INT,
    Desarrollador NVARCHAR(100),
    Estado NVARCHAR(50),
    FechaAlta DATETIME DEFAULT GETDATE(),
    Destacado BIT DEFAULT 0,
    Activo BIT DEFAULT 1
);

-- Tabla Pedidos
CREATE TABLE Pedidos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario NVARCHAR(100),
    Estado NVARCHAR(50),
    FechaPedido DATETIME DEFAULT GETDATE(),
    FechaEnvio DATETIME NULL,
    FechaEntrega DATETIME NULL,
    DireccionEnvio NVARCHAR(500),
    CiudadEnvio NVARCHAR(100),
    CodigoPostalEnvio NVARCHAR(20),
    PaisEnvio NVARCHAR(100),
    TelefonoContacto NVARCHAR(50),
    Subtotal DECIMAL(10,2),
    IVA DECIMAL(10,2),
    GastosEnvio DECIMAL(10,2),
    Total DECIMAL(10,2),
    Comentarios NVARCHAR(MAX)
);

-- Tabla DetallesPedido
CREATE TABLE DetallesPedido (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PedidoId INT FOREIGN KEY REFERENCES Pedidos(Id),
    JuegoId INT FOREIGN KEY REFERENCES Juegos(Id),
    TituloJuego NVARCHAR(200),
    ImagenJuego NVARCHAR(500),
    PrecioUnitario DECIMAL(10,2),
    Cantidad INT
);

-- ============================================
-- Datos de ejemplo: Consolas
-- ============================================
INSERT INTO Consolas (Nombre, ImagenUrl, Fabricante, AnioLanzamiento, Descripcion, Orden, Activa) VALUES
('NES', '/Content/images/consolas/nes.png', 'Nintendo', 1983, 'Nintendo Entertainment System - La consola que revolucionó la industria', 1, 1),
('SNES', '/Content/images/consolas/snes.png', 'Nintendo', 1990, 'Super Nintendo - 16 bits de pura diversión', 2, 1),
('Mega Drive', '/Content/images/consolas/megadrive.png', 'SEGA', 1988, 'SEGA Genesis - BLAST PROCESSING!', 3, 1),
('PlayStation', '/Content/images/consolas/ps1.png', 'Sony', 1994, 'La primera PlayStation que cambió todo', 4, 1),
('Nintendo 64', '/Content/images/consolas/n64.png', 'Nintendo', 1996, 'El poder de los 64 bits', 5, 1),
('Game Boy', '/Content/images/consolas/gameboy.png', 'Nintendo', 1989, 'Portátil legendaria', 6, 1);

-- ============================================
-- Datos de ejemplo: Juegos
-- ============================================
INSERT INTO Juegos (Titulo, Descripcion, Precio, Stock, ImagenUrl, ConsolaId, Genero, AnioLanzamiento, Desarrollador, Estado, Destacado, Activo) VALUES
('Super Mario Bros 3', 'La mejor aventura de Mario en NES', 24.99, 5, '/Content/images/juegos/smb3.png', 1, 'Plataformas', 1988, 'Nintendo', 'Bueno', 1, 1),
('The Legend of Zelda', 'La aventura épica original', 29.99, 3, '/Content/images/juegos/zelda.png', 1, 'Aventura', 1986, 'Nintendo', 'Bueno', 1, 1),
('Super Mario World', 'Mario en 16 bits con Yoshi', 34.99, 4, '/Content/images/juegos/smw.png', 2, 'Plataformas', 1990, 'Nintendo', 'Muy Bueno', 1, 1),
('Sonic the Hedgehog', 'Velocidad supersónica', 19.99, 6, '/Content/images/juegos/sonic.png', 3, 'Plataformas', 1991, 'SEGA', 'Bueno', 1, 1),
('Final Fantasy VII', 'El RPG que definió una generación', 49.99, 2, '/Content/images/juegos/ff7.png', 4, 'RPG', 1997, 'Square', 'Muy Bueno', 1, 1),
('Super Mario 64', 'Mario salta a las 3D', 39.99, 3, '/Content/images/juegos/sm64.png', 5, 'Plataformas', 1996, 'Nintendo', 'Bueno', 1, 1),
('Pokemon Rojo', 'Hazte con todos', 29.99, 4, '/Content/images/juegos/pokemon.png', 6, 'RPG', 1996, 'Game Freak', 'Bueno', 1, 1),
('Chrono Trigger', 'El mejor RPG de todos los tiempos', 59.99, 2, '/Content/images/juegos/chrono.png', 2, 'RPG', 1995, 'Square', 'Muy Bueno', 1, 1);
