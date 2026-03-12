using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SoplaElCartucho.Common.Models;

namespace SoplaElCartucho.Data.Repositories
{
    /// <summary>
    /// Repositorio para acceso a datos de Pedidos usando ADO.NET.
    /// ⚠️ ANTI-PATRÓN LEGACY: ADO.NET manual con transacciones explícitas.
    /// 📝 MIGRACIÓN: Usar Entity Framework Core con Unit of Work.
    /// </summary>
    public class PedidoRepository
    {
        public PedidoRepository() { }

        public List<Pedido> ObtenerTodos()
        {
            var pedidos = new List<Pedido>();

            using (var conexion = DatabaseHelper.ObtenerConexion())
            {
                conexion.Open();
                using (var comando = new SqlCommand(
                    @"SELECT Id, NombreUsuario, Estado, FechaPedido, FechaEnvio, FechaEntrega, 
                      DireccionEnvio, CiudadEnvio, CodigoPostalEnvio, PaisEnvio, TelefonoContacto,
                      Subtotal, IVA, GastosEnvio, Total, Comentarios 
                      FROM Pedidos ORDER BY FechaPedido DESC", conexion))
                {
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var pedido = MapearPedido(reader);
                            pedidos.Add(pedido);
                        }
                    }
                }

                // Cargar detalles de cada pedido
                foreach (var pedido in pedidos)
                {
                    pedido.Detalles = ObtenerDetallesPedido(conexion, pedido.Id);
                }
            }

            return pedidos;
        }

        public Pedido ObtenerPorId(int id)
        {
            using (var conexion = DatabaseHelper.ObtenerConexion())
            {
                conexion.Open();
                using (var comando = new SqlCommand(
                    @"SELECT Id, NombreUsuario, Estado, FechaPedido, FechaEnvio, FechaEntrega, 
                      DireccionEnvio, CiudadEnvio, CodigoPostalEnvio, PaisEnvio, TelefonoContacto,
                      Subtotal, IVA, GastosEnvio, Total, Comentarios 
                      FROM Pedidos WHERE Id = @Id", conexion))
                {
                    comando.Parameters.AddWithValue("@Id", id);

                    using (var reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var pedido = MapearPedido(reader);
                            reader.Close();
                            pedido.Detalles = ObtenerDetallesPedido(conexion, pedido.Id);
                            return pedido;
                        }
                    }
                }
            }

            return null;
        }

        public List<Pedido> ObtenerPorUsuario(string nombreUsuario)
        {
            var pedidos = new List<Pedido>();

            using (var conexion = DatabaseHelper.ObtenerConexion())
            {
                conexion.Open();
                using (var comando = new SqlCommand(
                    @"SELECT Id, NombreUsuario, Estado, FechaPedido, FechaEnvio, FechaEntrega, 
                      DireccionEnvio, CiudadEnvio, CodigoPostalEnvio, PaisEnvio, TelefonoContacto,
                      Subtotal, IVA, GastosEnvio, Total, Comentarios 
                      FROM Pedidos WHERE NombreUsuario = @NombreUsuario ORDER BY FechaPedido DESC", conexion))
                {
                    comando.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);

                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pedidos.Add(MapearPedido(reader));
                        }
                    }
                }

                // Cargar detalles de cada pedido
                foreach (var pedido in pedidos)
                {
                    pedido.Detalles = ObtenerDetallesPedido(conexion, pedido.Id);
                }
            }

            return pedidos;
        }

        /// <summary>
        /// Inserta un pedido con sus detalles usando una transacción.
        /// ⚠️ ANTI-PATRÓN LEGACY: Transacción manual sin Unit of Work.
        /// </summary>
        public int Insertar(Pedido pedido)
        {
            using (var conexion = DatabaseHelper.ObtenerConexion())
            {
                conexion.Open();
                using (var transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        // Insertar pedido
                        int pedidoId;
                        using (var comando = new SqlCommand(
                            @"INSERT INTO Pedidos (NombreUsuario, Estado, FechaPedido, FechaEnvio, FechaEntrega, 
                              DireccionEnvio, CiudadEnvio, CodigoPostalEnvio, PaisEnvio, TelefonoContacto,
                              Subtotal, IVA, GastosEnvio, Total, Comentarios) 
                              VALUES (@NombreUsuario, @Estado, @FechaPedido, @FechaEnvio, @FechaEntrega, 
                              @DireccionEnvio, @CiudadEnvio, @CodigoPostalEnvio, @PaisEnvio, @TelefonoContacto,
                              @Subtotal, @IVA, @GastosEnvio, @Total, @Comentarios);
                              SELECT SCOPE_IDENTITY();", conexion, transaccion))
                        {
                            AgregarParametrosPedido(comando, pedido);
                            pedidoId = Convert.ToInt32(comando.ExecuteScalar());
                        }

                        // Insertar detalles
                        if (pedido.Detalles != null)
                        {
                            foreach (var detalle in pedido.Detalles)
                            {
                                using (var comandoDetalle = new SqlCommand(
                                    @"INSERT INTO DetallesPedido (PedidoId, JuegoId, TituloJuego, ImagenJuego, PrecioUnitario, Cantidad) 
                                      VALUES (@PedidoId, @JuegoId, @TituloJuego, @ImagenJuego, @PrecioUnitario, @Cantidad)", conexion, transaccion))
                                {
                                    comandoDetalle.Parameters.AddWithValue("@PedidoId", pedidoId);
                                    comandoDetalle.Parameters.AddWithValue("@JuegoId", detalle.JuegoId);
                                    comandoDetalle.Parameters.AddWithValue("@TituloJuego", detalle.TituloJuego ?? (object)DBNull.Value);
                                    comandoDetalle.Parameters.AddWithValue("@ImagenJuego", detalle.ImagenJuego ?? (object)DBNull.Value);
                                    comandoDetalle.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);
                                    comandoDetalle.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                                    comandoDetalle.ExecuteNonQuery();
                                }
                            }
                        }

                        transaccion.Commit();
                        return pedidoId;
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Actualizar(Pedido pedido)
        {
            using (var conexion = DatabaseHelper.ObtenerConexion())
            {
                conexion.Open();
                using (var comando = new SqlCommand(
                    @"UPDATE Pedidos SET NombreUsuario = @NombreUsuario, Estado = @Estado, 
                      FechaPedido = @FechaPedido, FechaEnvio = @FechaEnvio, FechaEntrega = @FechaEntrega, 
                      DireccionEnvio = @DireccionEnvio, CiudadEnvio = @CiudadEnvio, 
                      CodigoPostalEnvio = @CodigoPostalEnvio, PaisEnvio = @PaisEnvio, 
                      TelefonoContacto = @TelefonoContacto, Subtotal = @Subtotal, IVA = @IVA, 
                      GastosEnvio = @GastosEnvio, Total = @Total, Comentarios = @Comentarios 
                      WHERE Id = @Id", conexion))
                {
                    comando.Parameters.AddWithValue("@Id", pedido.Id);
                    AgregarParametrosPedido(comando, pedido);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarEstado(int id, string nuevoEstado)
        {
            using (var conexion = DatabaseHelper.ObtenerConexion())
            {
                conexion.Open();

                string sql = "UPDATE Pedidos SET Estado = @Estado";

                if (nuevoEstado == "Enviado")
                    sql += ", FechaEnvio = @Fecha";
                else if (nuevoEstado == "Entregado")
                    sql += ", FechaEntrega = @Fecha";

                sql += " WHERE Id = @Id";

                using (var comando = new SqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@Id", id);
                    comando.Parameters.AddWithValue("@Estado", nuevoEstado);

                    if (nuevoEstado == "Enviado" || nuevoEstado == "Entregado")
                        comando.Parameters.AddWithValue("@Fecha", DateTime.Now);

                    comando.ExecuteNonQuery();
                }
            }
        }

        private List<DetallePedido> ObtenerDetallesPedido(SqlConnection conexion, int pedidoId)
        {
            var detalles = new List<DetallePedido>();

            using (var comando = new SqlCommand(
                @"SELECT Id, PedidoId, JuegoId, TituloJuego, ImagenJuego, PrecioUnitario, Cantidad 
                  FROM DetallesPedido WHERE PedidoId = @PedidoId", conexion))
            {
                comando.Parameters.AddWithValue("@PedidoId", pedidoId);

                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        detalles.Add(new DetallePedido
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PedidoId = reader.GetInt32(reader.GetOrdinal("PedidoId")),
                            JuegoId = reader.GetInt32(reader.GetOrdinal("JuegoId")),
                            TituloJuego = reader.IsDBNull(reader.GetOrdinal("TituloJuego")) ? null : reader.GetString(reader.GetOrdinal("TituloJuego")),
                            ImagenJuego = reader.IsDBNull(reader.GetOrdinal("ImagenJuego")) ? null : reader.GetString(reader.GetOrdinal("ImagenJuego")),
                            PrecioUnitario = reader.GetDecimal(reader.GetOrdinal("PrecioUnitario")),
                            Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad"))
                        });
                    }
                }
            }

            return detalles;
        }

        private void AgregarParametrosPedido(SqlCommand comando, Pedido pedido)
        {
            comando.Parameters.AddWithValue("@NombreUsuario", pedido.NombreUsuario ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", pedido.Estado ?? "Pendiente");
            comando.Parameters.AddWithValue("@FechaPedido", pedido.FechaPedido);
            comando.Parameters.AddWithValue("@FechaEnvio", pedido.FechaEnvio ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FechaEntrega", pedido.FechaEntrega ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@DireccionEnvio", pedido.DireccionEnvio ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CiudadEnvio", pedido.CiudadEnvio ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CodigoPostalEnvio", pedido.CodigoPostalEnvio ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PaisEnvio", pedido.PaisEnvio ?? "España");
            comando.Parameters.AddWithValue("@TelefonoContacto", pedido.TelefonoContacto ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Subtotal", pedido.Subtotal);
            comando.Parameters.AddWithValue("@IVA", pedido.IVA);
            comando.Parameters.AddWithValue("@GastosEnvio", pedido.GastosEnvio);
            comando.Parameters.AddWithValue("@Total", pedido.Total);
            comando.Parameters.AddWithValue("@Comentarios", pedido.Comentarios ?? (object)DBNull.Value);
        }

        private Pedido MapearPedido(SqlDataReader reader)
        {
            return new Pedido
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                NombreUsuario = reader.IsDBNull(reader.GetOrdinal("NombreUsuario")) ? null : reader.GetString(reader.GetOrdinal("NombreUsuario")),
                Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? null : reader.GetString(reader.GetOrdinal("Estado")),
                FechaPedido = reader.GetDateTime(reader.GetOrdinal("FechaPedido")),
                FechaEnvio = reader.IsDBNull(reader.GetOrdinal("FechaEnvio")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaEnvio")),
                FechaEntrega = reader.IsDBNull(reader.GetOrdinal("FechaEntrega")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaEntrega")),
                DireccionEnvio = reader.IsDBNull(reader.GetOrdinal("DireccionEnvio")) ? null : reader.GetString(reader.GetOrdinal("DireccionEnvio")),
                CiudadEnvio = reader.IsDBNull(reader.GetOrdinal("CiudadEnvio")) ? null : reader.GetString(reader.GetOrdinal("CiudadEnvio")),
                CodigoPostalEnvio = reader.IsDBNull(reader.GetOrdinal("CodigoPostalEnvio")) ? null : reader.GetString(reader.GetOrdinal("CodigoPostalEnvio")),
                PaisEnvio = reader.IsDBNull(reader.GetOrdinal("PaisEnvio")) ? null : reader.GetString(reader.GetOrdinal("PaisEnvio")),
                TelefonoContacto = reader.IsDBNull(reader.GetOrdinal("TelefonoContacto")) ? null : reader.GetString(reader.GetOrdinal("TelefonoContacto")),
                Subtotal = reader.GetDecimal(reader.GetOrdinal("Subtotal")),
                IVA = reader.GetDecimal(reader.GetOrdinal("IVA")),
                GastosEnvio = reader.GetDecimal(reader.GetOrdinal("GastosEnvio")),
                Total = reader.GetDecimal(reader.GetOrdinal("Total")),
                Comentarios = reader.IsDBNull(reader.GetOrdinal("Comentarios")) ? null : reader.GetString(reader.GetOrdinal("Comentarios"))
            };
        }
    }
}
