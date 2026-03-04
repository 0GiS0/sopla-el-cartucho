using System;
using System.Collections.Generic;

namespace SoplaElCartucho.Data.Repositories
{
    public class PedidoRepository
    {
        public PedidoRepository() { }

        public List<object> ObtenerTodos() { return new List<object>(); }

        public object ObtenerPorId(int id) { return null; }

        public List<object> ObtenerPorUsuario(string nombreUsuario) { return new List<object>(); }

        public int Insertar(object pedido) { return 0; }

        public void Actualizar(object pedido) { }

        public void ActualizarEstado(int id, string nuevoEstado) { }
    }
}
