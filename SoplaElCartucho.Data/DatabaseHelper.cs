using System;
using System.Configuration;
using System.Data.SqlClient;

namespace SoplaElCartucho.Data
{
    /// <summary>
    /// Helper estático para obtener conexiones a la base de datos.
    /// ⚠️ ANTI-PATRÓN LEGACY: Clase estática sin abstracción.
    /// 📝 MIGRACIÓN: Usar inyección de dependencias con IDbConnectionFactory.
    /// </summary>
    public static class DatabaseHelper
    {
        private static readonly string _connectionString;

        static DatabaseHelper()
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings["SoplaElCartuchoDb"];
            if (connectionStringSettings == null)
            {
                throw new ConfigurationErrorsException(
                    "No se encontró la cadena de conexión 'SoplaElCartuchoDb' en el archivo de configuración.");
            }
            _connectionString = connectionStringSettings.ConnectionString;
        }

        /// <summary>
        /// Obtiene una nueva conexión a la base de datos.
        /// ⚠️ IMPORTANTE: El llamador es responsable de cerrar la conexión (usar 'using').
        /// </summary>
        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Obtiene la cadena de conexión configurada.
        /// </summary>
        public static string ObtenerCadenaConexion()
        {
            return _connectionString;
        }
    }
}
