using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SoplaElCartucho.Common.Models;

namespace SoplaElCartucho.Data.Repositories
{
    /// <summary>
    /// Repositorio para acceso a datos de Juegos usando ADO.NET.
    /// ⚠️ ANTI-PATRÓN LEGACY: ADO.NET manual sin ORM.
    /// 📝 MIGRACIÓN: Usar Entity Framework Core o Dapper.
    /// </summary>
    public class JuegoRepository
    {
        public JuegoRepository() { }

        public List<Juego> ObtenerTodos()
        {
            var juegos = new List<Juego>();

            using (var conexion = DatabaseHelper.ObtenerConexion())
            {
                conexion.Open();
                using (var comando = new SqlCommand(
                    @"SELECT Id, Titulo, Descripcion, Precio, Stock, ImagenUrl, ConsolaId, Genero, 
                      AnioLanzamiento, Desarrollador, Estado, FechaAlta, Destacado, Activo 
                      FROM Juegos WHERE Activo = 1", conexion))
                {
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            juegos.Add(MapearJuego(reader));
                        }
                    }
                }
            }

            return juegos;
        }

        public Juego ObtenerPorId(int id)
        {
            using (var conexion = DatabaseHelper.ObtenerConexion())
            {
                conexion.Open();
                using (var comando = new SqlCommand(
                    @"SELECT Id, Titulo, Descripcion, Precio, Stock, ImagenUrl, ConsolaId, Genero, 
                      AnioLanzamiento, Desarrollador, Estado, FechaAlta, Destacado, Activo 
                      FROM Juegos WHERE Id = @Id", conexion))
                {
                    comando.Parameters.AddWithValue("@Id", id);

                    using (var reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapearJuego(reader);
                        }
                    }
                }
            }

            return null;
        }

        public List<Juego> ObtenerPorConsola(int consolaId)
        {
            var juegos = new List<Juego>();

            using (var conexion = DatabaseHelper.ObtenerConexion())
            {
                conexion.Open();
                using (var comando = new SqlCommand(
                    @"SELECT Id, Titulo, Descripcion, Precio, Stock, ImagenUrl, ConsolaId, Genero, 
                      AnioLanzamiento, Desarrollador, Estado, FechaAlta, Destacado, Activo 
                      FROM Juegos WHERE ConsolaId = @ConsolaId AND Activo = 1", conexion))
                {
                    comando.Parameters.AddWithValue("@ConsolaId", consolaId);

                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            juegos.Add(MapearJuego(reader));
                        }
                    }
                }
            }

            return juegos;
        }

        public List<Juego> ObtenerDestacados()
        {
            var juegos = new List<Juego>();

            using (var conexion = DatabaseHelper.ObtenerConexion())
            {
                conexion.Open();
                using (var comando = new SqlCommand(
                    @"SELECT Id, Titulo, Descripcion, Precio, Stock, ImagenUrl, ConsolaId, Genero, 
                      AnioLanzamiento, Desarrollador, Estado, FechaAlta, Destacado, Activo 
                      FROM Juegos WHERE Destacado = 1 AND Activo = 1", conexion))
                {
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            juegos.Add(MapearJuego(reader));
                        }
                    }
                }
            }

            return juegos;
        }

        public void Insertar(Juego juego)
        {
            using (var conexion = DatabaseHelper.ObtenerConexion())
            {
                conexion.Open();
                using (var comando = new SqlCommand(
                    @"INSERT INTO Juegos (Titulo, Descripcion, Precio, Stock, ImagenUrl, ConsolaId, Genero, 
                      AnioLanzamiento, Desarrollador, Estado, FechaAlta, Destacado, Activo) 
                      VALUES (@Titulo, @Descripcion, @Precio, @Stock, @ImagenUrl, @ConsolaId, @Genero, 
                      @AnioLanzamiento, @Desarrollador, @Estado, @FechaAlta, @Destacado, @Activo)", conexion))
                {
                    AgregarParametrosJuego(comando, juego);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(Juego juego)
        {
            using (var conexion = DatabaseHelper.ObtenerConexion())
            {
                conexion.Open();
                using (var comando = new SqlCommand(
                    @"UPDATE Juegos SET Titulo = @Titulo, Descripcion = @Descripcion, Precio = @Precio, 
                      Stock = @Stock, ImagenUrl = @ImagenUrl, ConsolaId = @ConsolaId, Genero = @Genero, 
                      AnioLanzamiento = @AnioLanzamiento, Desarrollador = @Desarrollador, Estado = @Estado, 
                      FechaAlta = @FechaAlta, Destacado = @Destacado, Activo = @Activo 
                      WHERE Id = @Id", conexion))
                {
                    comando.Parameters.AddWithValue("@Id", juego.Id);
                    AgregarParametrosJuego(comando, juego);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarStock(int id, int nuevoStock)
        {
            using (var conexion = DatabaseHelper.ObtenerConexion())
            {
                conexion.Open();
                using (var comando = new SqlCommand("UPDATE Juegos SET Stock = @Stock WHERE Id = @Id", conexion))
                {
                    comando.Parameters.AddWithValue("@Id", id);
                    comando.Parameters.AddWithValue("@Stock", nuevoStock);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(int id)
        {
            using (var conexion = DatabaseHelper.ObtenerConexion())
            {
                conexion.Open();
                using (var comando = new SqlCommand("UPDATE Juegos SET Activo = 0 WHERE Id = @Id", conexion))
                {
                    comando.Parameters.AddWithValue("@Id", id);
                    comando.ExecuteNonQuery();
                }
            }
        }

        private void AgregarParametrosJuego(SqlCommand comando, Juego juego)
        {
            comando.Parameters.AddWithValue("@Titulo", juego.Titulo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Descripcion", juego.Descripcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Precio", juego.Precio);
            comando.Parameters.AddWithValue("@Stock", juego.Stock);
            comando.Parameters.AddWithValue("@ImagenUrl", juego.ImagenUrl ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ConsolaId", juego.ConsolaId);
            comando.Parameters.AddWithValue("@Genero", juego.Genero ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@AnioLanzamiento", juego.AnioLanzamiento);
            comando.Parameters.AddWithValue("@Desarrollador", juego.Desarrollador ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", juego.Estado ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FechaAlta", juego.FechaAlta ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Destacado", juego.Destacado);
            comando.Parameters.AddWithValue("@Activo", juego.Activo);
        }

        private Juego MapearJuego(SqlDataReader reader)
        {
            return new Juego
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Titulo = reader.IsDBNull(reader.GetOrdinal("Titulo")) ? null : reader.GetString(reader.GetOrdinal("Titulo")),
                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                Stock = reader.IsDBNull(reader.GetOrdinal("Stock")) ? 0 : reader.GetInt32(reader.GetOrdinal("Stock")),
                ImagenUrl = reader.IsDBNull(reader.GetOrdinal("ImagenUrl")) ? null : reader.GetString(reader.GetOrdinal("ImagenUrl")),
                ConsolaId = reader.GetInt32(reader.GetOrdinal("ConsolaId")),
                Genero = reader.IsDBNull(reader.GetOrdinal("Genero")) ? null : reader.GetString(reader.GetOrdinal("Genero")),
                AnioLanzamiento = reader.IsDBNull(reader.GetOrdinal("AnioLanzamiento")) ? 0 : reader.GetInt32(reader.GetOrdinal("AnioLanzamiento")),
                Desarrollador = reader.IsDBNull(reader.GetOrdinal("Desarrollador")) ? null : reader.GetString(reader.GetOrdinal("Desarrollador")),
                Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? null : reader.GetString(reader.GetOrdinal("Estado")),
                FechaAlta = reader.IsDBNull(reader.GetOrdinal("FechaAlta")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaAlta")),
                Destacado = reader.IsDBNull(reader.GetOrdinal("Destacado")) ? false : reader.GetBoolean(reader.GetOrdinal("Destacado")),
                Activo = reader.IsDBNull(reader.GetOrdinal("Activo")) ? false : reader.GetBoolean(reader.GetOrdinal("Activo"))
            };
        }
    }
}
