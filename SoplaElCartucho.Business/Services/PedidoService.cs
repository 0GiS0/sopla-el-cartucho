using System;
using System.Collections.Generic;
using SoplaElCartucho.Data.Repositories;

namespace SoplaElCartucho.Business.Services
{
    public class PedidoService
    {
        private readonly PedidoRepository _pedidoRepository;

        public PedidoService()
        {
            _pedidoRepository = new PedidoRepository();
        }

        public List<object> ObtenerPedidos() { return _pedidoRepository.ObtenerTodos(); }

        public object ObtenerPedidoPorId(int id) { return _pedidoRepository.ObtenerPorId(id); }

        public List<object> ObtenerPedidosPorUsuario(string nombreUsuario) { return _pedidoRepository.ObtenerPorUsuario(nombreUsuario); }

        public int CrearPedido(object pedido) { return _pedidoRepository.Insertar(pedido); }

        public void ActualizarEstado(int id, string nuevoEstado) { _pedidoRepository.ActualizarEstado(id, nuevoEstado); }
    }
}
