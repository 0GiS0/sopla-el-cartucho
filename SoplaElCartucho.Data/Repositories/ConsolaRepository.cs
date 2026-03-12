using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SoplaElCartucho.Common.Models;

namespace SoplaElCartucho.Data.Repositories
{
    /// <summary>
    /// Repositorio para acceso a datos de Consolas usando ADO.NET.
    /// ⚠️ ANTI-PATRÓN LEGACY: ADO.NET manual sin ORM.
    /// 📝 MIGRACIÓN: Usar Entity Framework Core o Dapper.
    /// </summary>
    public class ConsolaRepository
    {
        public ConsolaRepository() { }

        public List<Consola> ObtenerTodas()
        {
            var consolas = new List<Consola>();

            using (var conexion = DatabaseHelper.ObtenerConexion())
            {
                conexion.Open();
                using (var comando = new SqlCommand("SELECT Id, Nombre, ImagenUrl, Fabricante, AnioLanzamiento, Descripcion, Orden, Activa FROM Consolas", conexion))
                {
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            consolas.Add(MapearConsola(reader));
                        }
                    }
                }
            }

            return consolas;
        }

        public Consola ObtenerPorId(int id)
        {
            using (var conexion = DatabaseHelper.ObtenerConexion())
            {
                conexion.Open();
                using (var comando = new SqlCommand("SELECT Id, Nombre, ImagenUrl, Fabricante, AnioLanzamiento, Descripcion, Orden, Activa FROM Consolas WHERE Id = @Id", conexion))
                {
                    comando.Parameters.AddWithValue("@Id", id);

                    using (var reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapearConsola(reader);
                        }
                    }
                }
            }

            return null;
        }

        public List<Consola> ObtenerActivas()
        {
            var consolas = new List<Consola>();

            using (var conexion = DatabaseHelper.ObtenerConexion())
            {
                conexion.Open();
                using (var comando = new SqlCommand("SELECT Id, Nombre, ImagenUrl, Fabricante, AnioLanzamiento, Descripcion, Orden, Activa FROM Consolas WHERE Activa = 1 ORDER BY Orden", conexion))
                {
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            consolas.Add(MapearConsola(reader));
                        }
                    }
                }
            }

            return consolas;
        }

        public void Insertar(Consola consola)
        {
            using (var conexion = DatabaseHelper.ObtenerConexion())
            {
                conexion.Open();
                using (var comando = new SqlCommand(
                    @"INSERT INTO Consolas (Nombre, ImagenUrl, Fabricante, AnioLanzamiento, Descripcion, Orden, Activa) 
                      VALUES (@Nombre, @ImagenUrl, @Fabricante, @AnioLanzamiento, @Descripcion, @Orden, @Activa)", conexion))
                {
                    comando.Parameters.AddWithValue("@Nombre", consola.Nombre ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@ImagenUrl", consola.ImagenUrl ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@Fabricante", consola.Fabricante ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@AnioLanzamiento", consola.AnioLanzamiento);
                    comando.Parameters.AddWithValue("@Descripcion", consola.Descripcion ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@Orden", consola.Orden);
                    comando.Parameters.AddWithValue("@Activa", consola.Activa);

                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(Consola consola)
        {
            using (var conexion = DatabaseHelper.ObtenerConexion())
            {
                conexion.Open();
                using (var comando = new SqlCommand(
                    @"UPDATE Consolas SET Nombre = @Nombre, ImagenUrl = @ImagenUrl, Fabricante = @Fabricante, 
                      AnioLanzamiento = @AnioLanzamiento, Descripcion = @Descripcion, Orden = @Orden, Activa = @Activa 
                      WHERE Id = @Id", conexion))
                {
                    comando.Parameters.AddWithValue("@Id", consola.Id);
                    comando.Parameters.AddWithValue("@Nombre", consola.Nombre ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@ImagenUrl", consola.ImagenUrl ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@Fabricante", consola.Fabricante ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@AnioLanzamiento", consola.AnioLanzamiento);
                    comando.Parameters.AddWithValue("@Descripcion", consola.Descripcion ?? (object)DBNull.Value);
                    comando.Parameters.AddWithValue("@Orden", consola.Orden);
                    comando.Parameters.AddWithValue("@Activa", consola.Activa);

                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(int id)
        {
            using (var conexion = DatabaseHelper.ObtenerConexion())
            {
                conexion.Open();
                using (var comando = new SqlCommand("DELETE FROM Consolas WHERE Id = @Id", conexion))
                {
                    comando.Parameters.AddWithValue("@Id", id);
                    comando.ExecuteNonQuery();
                }
            }
        }

        private Consola MapearConsola(SqlDataReader reader)
        {
            return new Consola
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? null : reader.GetString(reader.GetOrdinal("Nombre")),
                ImagenUrl = reader.IsDBNull(reader.GetOrdinal("ImagenUrl")) ? null : reader.GetString(reader.GetOrdinal("ImagenUrl")),
                Fabricante = reader.IsDBNull(reader.GetOrdinal("Fabricante")) ? null : reader.GetString(reader.GetOrdinal("Fabricante")),
                AnioLanzamiento = reader.IsDBNull(reader.GetOrdinal("AnioLanzamiento")) ? 0 : reader.GetInt32(reader.GetOrdinal("AnioLanzamiento")),
                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                Orden = reader.IsDBNull(reader.GetOrdinal("Orden")) ? 0 : reader.GetInt32(reader.GetOrdinal("Orden")),
                Activa = reader.IsDBNull(reader.GetOrdinal("Activa")) ? false : reader.GetBoolean(reader.GetOrdinal("Activa"))
            };
        }
    }
}
