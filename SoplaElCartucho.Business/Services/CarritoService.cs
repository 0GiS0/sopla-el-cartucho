using System;
using System.Collections.Generic;

namespace SoplaElCartucho.Business.Services
{
    public class CarritoService
    {
        public CarritoService() { }

        public List<object> ObtenerItems(string sessionId) { return new List<object>(); }

        public void AgregarItem(string sessionId, int juegoId, int cantidad) { }

        public void ActualizarCantidad(string sessionId, int juegoId, int cantidad) { }

        public void EliminarItem(string sessionId, int juegoId) { }

        public void VaciarCarrito(string sessionId) { }

        public decimal CalcularTotal(string sessionId) { return 0m; }

        public int ObtenerCantidadItems(string sessionId) { return 0; }
    }
}
