using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SoplaElCartucho.Data
{
    /*
     * ============================================
     * 🎮 SOPLA EL CARTUCHO - DatabaseInitializer
     * ============================================
     * ⚠️ ANTI-PATRONES EN ESTA CLASE:
     * 1. Inicialización manual de base de datos (no usa migrations)
     * 2. Script SQL embebido como recurso (difícil de mantener)
     * 3. Clase estática sin abstracción
     * 
     * 📝 MIGRACIÓN A .NET 8:
     * - Usar Entity Framework Core Migrations
     * - Configurar en Program.cs con IHostApplicationLifetime
     * - Usar IDbContextFactory para crear contextos
     * ============================================
     */

    /// <summary>
    /// Inicializador de base de datos legacy.
    /// Comprueba si la base de datos existe y la crea si no.
    /// ⚠️ ANTI-PATRÓN: Esto se hacía así antes de Entity Framework Migrations.
    /// </summary>
    public static class DatabaseInitializer
    {
        private const string NOMBRE_BASE_DATOS = "SoplaElCartucho";

        /// <summary>
        /// Inicializa la base de datos si no existe.
        /// Debe llamarse desde Application_Start en Global.asax.cs
        /// </summary>
        public static void Inicializar()
        {
            try
            {
                if (!ExisteBaseDatos())
                {
                    Debug.WriteLine("📦 Base de datos no encontrada. Creando...");
                    CrearBaseDatos();
                    Debug.WriteLine("✅ Base de datos creada correctamente.");
                }
                else if (!ExistenTablas())
                {
                    Debug.WriteLine("📦 Base de datos existe pero faltan tablas. Creando tablas...");
                    CrearTablas();
                    Debug.WriteLine("✅ Tablas creadas correctamente.");
                }
                else
                {
                    Debug.WriteLine("✅ Base de datos ya existe.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error inicializando base de datos: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Comprueba si las tablas principales existen en la base de datos.
        /// ⚠️ LEGACY: Consulta directa a INFORMATION_SCHEMA
        /// </summary>
        private static bool ExistenTablas()
        {
            var connectionString = DatabaseHelper.ObtenerCadenaConexion();

            using (var conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                using (var comando = new SqlCommand(
                    "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Consolas'",
                    conexion))
                {
                    var count = (int)comando.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// Crea las tablas ejecutando el script de inicialización.
        /// Se usa cuando la base de datos existe pero las tablas no.
        /// ⚠️ LEGACY: Ejecución de script SQL manual
        /// </summary>
        private static void CrearTablas()
        {
            var script = ObtenerScriptCreacion();
            var connectionString = DatabaseHelper.ObtenerCadenaConexion();

            using (var conexion = new SqlConnection(connectionString))
            {
                conexion.Open();

                // Dividir el script por GO (separador de lotes en SQL Server)
                // ⚠️ LEGACY: SqlCommand no soporta GO, hay que dividir manualmente
                var lotes = script.Split(new[] { "\r\nGO\r\n", "\nGO\n", "\r\nGO", "GO\r\n" },
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (var lote in lotes)
                {
                    if (!string.IsNullOrWhiteSpace(lote))
                    {
                        using (var comando = new SqlCommand(lote, conexion))
                        {
                            comando.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Comprueba si la base de datos existe conectándose a master.
        /// ⚠️ LEGACY: Consulta directa a sys.databases
        /// </summary>
        private static bool ExisteBaseDatos()
        {
            var connectionString = ObtenerCadenaConexionMaster();

            using (var conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                using (var comando = new SqlCommand(
                    $"SELECT COUNT(*) FROM sys.databases WHERE name = '{NOMBRE_BASE_DATOS}'", 
                    conexion))
                {
                    var count = (int)comando.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// Crea la base de datos y ejecuta el script de inicialización.
        /// ⚠️ LEGACY: Ejecución de script SQL manual
        /// </summary>
        private static void CrearBaseDatos()
        {
            var connectionStringMaster = ObtenerCadenaConexionMaster();

            // Paso 1: Crear la base de datos
            using (var conexion = new SqlConnection(connectionStringMaster))
            {
                conexion.Open();
                using (var comando = new SqlCommand(
                    $"CREATE DATABASE [{NOMBRE_BASE_DATOS}]", 
                    conexion))
                {
                    comando.ExecuteNonQuery();
                }
            }

            // Paso 2: Ejecutar el script de creación de tablas y datos
            var script = ObtenerScriptCreacion();
            var connectionString = DatabaseHelper.ObtenerCadenaConexion();

            using (var conexion = new SqlConnection(connectionString))
            {
                conexion.Open();

                // Dividir el script por GO (separador de lotes en SQL Server)
                // ⚠️ LEGACY: SqlCommand no soporta GO, hay que dividir manualmente
                var lotes = script.Split(new[] { "\r\nGO\r\n", "\nGO\n", "\r\nGO", "GO\r\n" }, 
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (var lote in lotes)
                {
                    if (!string.IsNullOrWhiteSpace(lote))
                    {
                        using (var comando = new SqlCommand(lote, conexion))
                        {
                            comando.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Obtiene la cadena de conexión apuntando a master para crear la BD.
        /// </summary>
        private static string ObtenerCadenaConexionMaster()
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings["SoplaElCartuchoDb"];
            if (connectionStringSettings == null)
            {
                throw new ConfigurationErrorsException(
                    "No se encontró la cadena de conexión 'SoplaElCartuchoDb' en el archivo de configuración.");
            }

            // Reemplazar el catálogo inicial por master
            var builder = new SqlConnectionStringBuilder(connectionStringSettings.ConnectionString);
            builder.InitialCatalog = "master";
            return builder.ConnectionString;
        }

        /// <summary>
        /// Obtiene el script SQL de creación desde el recurso embebido o archivo.
        /// ⚠️ LEGACY: Cargar scripts como recursos embebidos
        /// </summary>
        private static string ObtenerScriptCreacion()
        {
            // Intentar cargar desde recurso embebido
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "SoplaElCartucho.Data.Scripts.CrearBaseDatos.sql";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }

            // Fallback: intentar cargar desde archivo en disco
            var rutaArchivo = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, 
                "bin", 
                "Scripts", 
                "CrearBaseDatos.sql");

            if (File.Exists(rutaArchivo))
            {
                return File.ReadAllText(rutaArchivo);
            }

            throw new FileNotFoundException(
                $"No se encontró el script de creación de base de datos. " +
                $"Recurso: {resourceName}, Archivo: {rutaArchivo}");
        }
    }
}
