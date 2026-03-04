/*
 * 🎮 SOPLA EL CARTUCHO - Script de Creación de Base de Datos
 * 
 * Este script crea la estructura de tablas para la aplicación.
 * Ejecutar en SQL Server / SQL Server Express / LocalDB.
 * 
 * ⚠️ ANTI-PATRONES EN ESTE SCRIPT:
 * 1. Tablas sin índices optimizados
 * 2. Sin constraints de FK explícitos en algunos casos
 * 3. Campos NVARCHAR(MAX) donde no es necesario
 * 4. Sin particionamiento para tablas grandes
 * 
 * 📝 MIGRACIÓN:
 * - Usar EF Core Migrations para gestión de esquema
 * - Añadir índices apropiados
 * - Implementar soft delete con campos IsDeleted
 * - Añadir campos de auditoría (CreatedAt, UpdatedAt, CreatedBy)
 */

-- Crear la base de datos
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'SoplaElCartucho')
BEGIN
    CREATE DATABASE SoplaElCartucho;
END
GO

USE SoplaElCartucho;
GO

-- =====================================================
-- TABLA: Consolas
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Consolas')
BEGIN
    CREATE TABLE Consolas (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        Fabricante NVARCHAR(100) NULL,
        AnioLanzamiento INT NULL,
        ImagenUrl NVARCHAR(500) NULL,
        Descripcion NVARCHAR(MAX) NULL,  -- ⚠️ LEGACY: MAX innecesario
        Orden INT DEFAULT 0,
        Activa BIT DEFAULT 1,
        
        -- ⚠️ LEGACY: Sin campos de auditoría
        -- CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
        -- UpdatedAt DATETIME2 NULL
    );
    
    PRINT '✓ Tabla Consolas creada';
END
GO

-- =====================================================
-- TABLA: Juegos
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Juegos')
BEGIN
    CREATE TABLE Juegos (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Titulo NVARCHAR(200) NOT NULL,
        Descripcion NVARCHAR(MAX) NULL,
        Precio DECIMAL(10,2) NOT NULL,
        Stock INT DEFAULT 0,
        ImagenUrl NVARCHAR(500) NULL,
        ConsolaId INT NOT NULL,
        Genero NVARCHAR(50) NULL,       -- ⚠️ LEGACY: Debería ser FK a tabla Generos
        AnioLanzamiento INT NULL,
        Desarrollador NVARCHAR(100) NULL,
        Estado NVARCHAR(20) DEFAULT 'BuenEstado',  -- ⚠️ LEGACY: Debería ser ENUM/tabla
        FechaAlta DATETIME DEFAULT GETDATE(),     -- ⚠️ LEGACY: Sin UTC
        Destacado BIT DEFAULT 0,
        Activo BIT DEFAULT 1,
        
        CONSTRAINT FK_Juegos_Consolas FOREIGN KEY (ConsolaId) 
            REFERENCES Consolas(Id)
    );
    
    -- ⚠️ LEGACY: Índice básico, faltarían índices compuestos
    CREATE INDEX IX_Juegos_ConsolaId ON Juegos(ConsolaId);
    CREATE INDEX IX_Juegos_Activo ON Juegos(Activo);
    
    PRINT '✓ Tabla Juegos creada';
END
GO

-- =====================================================
-- TABLA: Pedidos
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Pedidos')
BEGIN
    CREATE TABLE Pedidos (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        NombreUsuario NVARCHAR(256) NOT NULL,     -- ⚠️ LEGACY: Debería ser FK a Users
        Estado NVARCHAR(20) DEFAULT 'Pendiente',  -- ⚠️ LEGACY: Magic string
        FechaPedido DATETIME DEFAULT GETDATE(),
        FechaEnvio DATETIME NULL,
        FechaEntrega DATETIME NULL,
        
        -- Datos de envío (⚠️ ANTI-PATRÓN: Deberían estar en tabla separada)
        DireccionEnvio NVARCHAR(500) NULL,
        CiudadEnvio NVARCHAR(100) NULL,
        CodigoPostalEnvio NVARCHAR(20) NULL,
        PaisEnvio NVARCHAR(100) DEFAULT 'España',
        TelefonoContacto NVARCHAR(50) NULL,
        
        -- Totales (⚠️ ANTI-PATRÓN: Denormalización)
        Subtotal DECIMAL(10,2) DEFAULT 0,
        IVA DECIMAL(10,2) DEFAULT 0,
        GastosEnvio DECIMAL(10,2) DEFAULT 0,
        Total DECIMAL(10,2) DEFAULT 0,
        
        Comentarios NVARCHAR(MAX) NULL
    );
    
    CREATE INDEX IX_Pedidos_NombreUsuario ON Pedidos(NombreUsuario);
    CREATE INDEX IX_Pedidos_Estado ON Pedidos(Estado);
    CREATE INDEX IX_Pedidos_FechaPedido ON Pedidos(FechaPedido DESC);
    
    PRINT '✓ Tabla Pedidos creada';
END
GO

-- =====================================================
-- TABLA: DetallesPedido
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DetallesPedido')
BEGIN
    CREATE TABLE DetallesPedido (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        PedidoId INT NOT NULL,
        JuegoId INT NOT NULL,
        
        -- ⚠️ ANTI-PATRÓN: Datos copiados del juego (snapshot)
        TituloJuego NVARCHAR(200) NOT NULL,
        ImagenJuego NVARCHAR(500) NULL,
        PrecioUnitario DECIMAL(10,2) NOT NULL,
        
        Cantidad INT NOT NULL DEFAULT 1,
        
        CONSTRAINT FK_DetallesPedido_Pedidos FOREIGN KEY (PedidoId) 
            REFERENCES Pedidos(Id) ON DELETE CASCADE,
        CONSTRAINT FK_DetallesPedido_Juegos FOREIGN KEY (JuegoId) 
            REFERENCES Juegos(Id)
    );
    
    CREATE INDEX IX_DetallesPedido_PedidoId ON DetallesPedido(PedidoId);
    
    PRINT '✓ Tabla DetallesPedido creada';
END
GO

-- =====================================================
-- DATOS INICIALES: Consolas
-- =====================================================
IF NOT EXISTS (SELECT * FROM Consolas)
BEGIN
    INSERT INTO Consolas (Nombre, Fabricante, AnioLanzamiento, ImagenUrl, Descripcion, Orden, Activa)
    VALUES 
        ('NES', 'Nintendo', 1983, '/Content/images/consolas/nes.png', 'Nintendo Entertainment System - La consola que salvó la industria del videojuego', 1, 1),
        ('SNES', 'Nintendo', 1990, '/Content/images/consolas/snes.png', 'Super Nintendo Entertainment System - 16 bits de pura nostalgia', 2, 1),
        ('Mega Drive', 'SEGA', 1988, '/Content/images/consolas/megadrive.png', 'Genesis en América - La rival de Nintendo en los 90', 3, 1),
        ('PlayStation', 'Sony', 1994, '/Content/images/consolas/ps1.png', 'La primera PlayStation - Revolución con CD-ROM', 4, 1),
        ('Nintendo 64', 'Nintendo', 1996, '/Content/images/consolas/n64.png', 'El último cartucho de Nintendo en consolas de sobremesa', 5, 1),
        ('Game Boy', 'Nintendo', 1989, '/Content/images/consolas/gameboy.png', 'La portátil que lo cambió todo - Verde y gloriosa', 6, 1);
    
    PRINT '✓ Consolas insertadas';
END
GO

-- =====================================================
-- DATOS INICIALES: Juegos
-- =====================================================
IF NOT EXISTS (SELECT * FROM Juegos)
BEGIN
    -- NES (ConsolaId = 1)
    INSERT INTO Juegos (Titulo, ConsolaId, Precio, Stock, Genero, AnioLanzamiento, Desarrollador, Estado, Destacado, Activo, Descripcion)
    VALUES 
        ('Super Mario Bros 3', 1, 29.99, 5, 'Plataformas', 1988, 'Nintendo', 'BuenEstado', 1, 1, 'El mejor Mario de NES. Traje de mapache incluido. 💨'),
        ('Mega Man 2', 1, 34.99, 3, 'Acción', 1988, 'Capcom', 'BuenEstado', 0, 1, 'Dr. Wily nunca aprende. 8 Robot Masters te esperan.'),
        ('Castlevania', 1, 39.99, 2, 'Acción', 1986, 'Konami', 'Usado', 0, 1, 'Simon Belmont vs Drácula. ¡Látigo en mano!'),
        ('The Legend of Zelda', 1, 49.99, 1, 'Aventura', 1986, 'Nintendo', 'BuenEstado', 1, 1, 'It''s dangerous to go alone! Take this.'),
        
        -- SNES (ConsolaId = 2)
        ('The Legend of Zelda: A Link to the Past', 2, 44.99, 4, 'Aventura', 1991, 'Nintendo', 'BuenEstado', 1, 1, 'Hyrule en 16 bits. Perfección absoluta.'),
        ('Super Metroid', 2, 54.99, 2, 'Acción', 1994, 'Nintendo', 'Nuevo', 0, 1, 'El bebé Metroid... *sniff*'),
        ('Chrono Trigger', 2, 89.99, 1, 'RPG', 1995, 'Square', 'BuenEstado', 1, 1, 'Viaja en el tiempo. Salva el mundo. El mejor RPG de la historia.'),
        ('Super Mario World', 2, 39.99, 6, 'Plataformas', 1990, 'Nintendo', 'BuenEstado', 1, 1, 'Yoshi apareció aquí por primera vez. 🦖'),
        
        -- Mega Drive (ConsolaId = 3)
        ('Sonic the Hedgehog 2', 3, 24.99, 8, 'Plataformas', 1992, 'SEGA', 'BuenEstado', 1, 1, 'Tails debuta. Velocidad máxima. SEGAAAA!'),
        ('Streets of Rage 2', 3, 29.99, 4, 'Beat''em up', 1992, 'SEGA', 'Usado', 0, 1, 'Axel y Blaze contra Mr. X. Cooperativo legendario.'),
        ('Gunstar Heroes', 3, 44.99, 2, 'Acción', 1993, 'Treasure', 'BuenEstado', 0, 1, 'Treasure en su máximo esplendor. Explosiones everywhere.'),
        
        -- PlayStation (ConsolaId = 4)
        ('Final Fantasy VII', 4, 49.99, 3, 'RPG', 1997, 'Square', 'BuenEstado', 1, 1, 'Cloud, Aerith, Sephiroth... Lágrimas garantizadas.'),
        ('Metal Gear Solid', 4, 39.99, 5, 'Acción', 1998, 'Konami', 'BuenEstado', 0, 1, 'Snake? SNAKE?! SNAAAAKE!!!'),
        ('Resident Evil 2', 4, 44.99, 2, 'Survival Horror', 1998, 'Capcom', 'Usado', 0, 1, 'Leon y Claire en Raccoon City. Terror puro.'),
        ('Crash Bandicoot', 4, 29.99, 4, 'Plataformas', 1996, 'Naughty Dog', 'BuenEstado', 0, 1, 'WOAH! La mascota de PlayStation.'),
        
        -- Nintendo 64 (ConsolaId = 5)
        ('GoldenEye 007', 5, 34.99, 6, 'FPS', 1997, 'Rare', 'BuenEstado', 1, 1, '4 jugadores, pantalla dividida, no screen-peeking!'),
        ('Super Mario 64', 5, 39.99, 4, 'Plataformas', 1996, 'Nintendo', 'BuenEstado', 0, 1, 'Yahoo! Wahoo! Mama mia! Mario en 3D.'),
        ('The Legend of Zelda: Ocarina of Time', 5, 59.99, 2, 'Aventura', 1998, 'Nintendo', 'Nuevo', 1, 1, 'El mejor juego de todos los tiempos según muchos.'),
        ('Mario Kart 64', 5, 34.99, 5, 'Carreras', 1996, 'Nintendo', 'BuenEstado', 0, 1, 'Plátanos, caparazones y destrucción de amistades.'),
        
        -- Game Boy (ConsolaId = 6)
        ('Pokémon Red', 6, 29.99, 7, 'RPG', 1996, 'Game Freak', 'BuenEstado', 1, 1, 'Gotta catch ''em all! Elige a Charmander.'),
        ('Tetris', 6, 14.99, 10, 'Puzzle', 1989, 'Nintendo', 'Usado', 0, 1, 'El juego más adictivo de la historia. Bip bip bip...'),
        ('The Legend of Zelda: Link''s Awakening', 6, 34.99, 3, 'Aventura', 1993, 'Nintendo', 'BuenEstado', 0, 1, 'Zelda en Game Boy. El Pez del Viento te espera.');
    
    PRINT '✓ Juegos insertados';
END
GO

-- =====================================================
-- STORED PROCEDURE: Obtener Juegos por Consola
-- ⚠️ LEGACY: La lógica debería estar en la aplicación
-- =====================================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_ObtenerJuegosPorConsola')
    DROP PROCEDURE sp_ObtenerJuegosPorConsola;
GO

CREATE PROCEDURE sp_ObtenerJuegosPorConsola
    @ConsolaId INT = NULL,
    @SoloActivos BIT = 1,
    @SoloDestacados BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    
    -- ⚠️ LEGACY: Stored Procedure con lógica de negocio
    -- 📝 MIGRACIÓN: Mover a consultas LINQ en C#
    
    SELECT 
        j.Id,
        j.Titulo,
        j.Descripcion,
        j.Precio,
        j.Stock,
        j.ImagenUrl,
        j.Genero,
        j.AnioLanzamiento,
        j.Desarrollador,
        j.Estado,
        j.Destacado,
        c.Nombre AS NombreConsola,
        c.Fabricante
    FROM Juegos j
    INNER JOIN Consolas c ON j.ConsolaId = c.Id
    WHERE 
        (@ConsolaId IS NULL OR j.ConsolaId = @ConsolaId)
        AND (@SoloActivos = 0 OR j.Activo = 1)
        AND (@SoloDestacados = 0 OR j.Destacado = 1)
    ORDER BY 
        j.Destacado DESC,
        j.Titulo ASC;
END
GO

PRINT '✓ Stored Procedures creados';
PRINT '';
PRINT '🎮 ¡Base de datos SoplaElCartucho creada correctamente! 💨';
PRINT '   - 6 Consolas';
PRINT '   - 22 Juegos';
PRINT '';
PRINT '⚠️  Recuerda: Este es un proyecto LEGACY para demostración de migración.';
GO
