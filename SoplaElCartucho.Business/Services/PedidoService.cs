using System;
using System.Collections.Generic;
using SoplaElCartucho.Common.Models;
using SoplaElCartucho.Data.Repositories;

namespace SoplaElCartucho.Business.Services
{
    /// <summary>
    /// Servicio de negocio para operaciones de pedidos.
    /// ⚠️ ANTI-PATRÓN LEGACY: Sin inyección de dependencias, new() directo.
    /// 📝 MIGRACIÓN: Usar constructor con interfaces para DI.
    /// </summary>
    public class PedidoService
    {
        private readonly PedidoRepository _pedidoRepository;

        public PedidoService()
        {
            _pedidoRepository = new PedidoRepository();
        }

        public List<Pedido> ObtenerPedidos() 
        { 
            return _pedidoRepository.ObtenerTodos(); 
        }

        public Pedido ObtenerPedidoPorId(int id) 
        { 
            return _pedidoRepository.ObtenerPorId(id); 
        }

        public List<Pedido> ObtenerPedidosPorUsuario(string nombreUsuario) 
        { 
            return _pedidoRepository.ObtenerPorUsuario(nombreUsuario); 
        }

        public int CrearPedido(Pedido pedido) 
        { 
            return _pedidoRepository.Insertar(pedido); 
        }

        public void ActualizarEstado(int id, string nuevoEstado) 
        { 
            _pedidoRepository.ActualizarEstado(id, nuevoEstado); 
        }
    }
}
