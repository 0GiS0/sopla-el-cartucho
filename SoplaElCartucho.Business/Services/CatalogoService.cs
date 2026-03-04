using System;
using System.Collections.Generic;
using SoplaElCartucho.Data.Repositories;

namespace SoplaElCartucho.Business.Services
{
    public class CatalogoService
    {
        private readonly JuegoRepository _juegoRepository;
        private readonly ConsolaRepository _consolaRepository;

        public CatalogoService()
        {
            _juegoRepository = new JuegoRepository();
            _consolaRepository = new ConsolaRepository();
        }

        public List<object> ObtenerJuegos() { return _juegoRepository.ObtenerTodos(); }

        public object ObtenerJuegoPorId(int id) { return _juegoRepository.ObtenerPorId(id); }

        public List<object> ObtenerJuegosPorConsola(int consolaId) { return _juegoRepository.ObtenerPorConsola(consolaId); }

        public List<object> ObtenerConsolas() { return _consolaRepository.ObtenerTodas(); }

        public List<object> ObtenerConsolasActivas() { return _consolaRepository.ObtenerActivas(); }
    }
}
