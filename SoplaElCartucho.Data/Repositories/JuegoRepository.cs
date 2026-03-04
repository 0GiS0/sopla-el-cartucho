using System;
using System.Collections.Generic;

namespace SoplaElCartucho.Data.Repositories
{
    public class JuegoRepository
    {
        public JuegoRepository() { }

        public List<object> ObtenerTodos() { return new List<object>(); }

        public object ObtenerPorId(int id) { return null; }

        public List<object> ObtenerPorConsola(int consolaId) { return new List<object>(); }

        public void Insertar(object juego) { }

        public void Actualizar(object juego) { }

        public void Eliminar(int id) { }
    }
}
