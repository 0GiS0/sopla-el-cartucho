using System;
using System.Collections.Generic;
using SoplaElCartucho.Common.Models;
using SoplaElCartucho.Data.Repositories;

namespace SoplaElCartucho.Business.Services
{
    /// <summary>
    /// Servicio de negocio para operaciones del catálogo.
    /// ⚠️ ANTI-PATRÓN LEGACY: Sin inyección de dependencias, new() directo.
    /// 📝 MIGRACIÓN: Usar constructor con interfaces para DI.
    /// </summary>
    public class CatalogoService
    {
        private readonly JuegoRepository _juegoRepository;
        private readonly ConsolaRepository _consolaRepository;

        public CatalogoService()
        {
            _juegoRepository = new JuegoRepository();
            _consolaRepository = new ConsolaRepository();
        }

        public List<Juego> ObtenerJuegos() 
        { 
            return _juegoRepository.ObtenerTodos(); 
        }

        public Juego ObtenerJuegoPorId(int id) 
        { 
            return _juegoRepository.ObtenerPorId(id); 
        }

        public List<Juego> ObtenerJuegosPorConsola(int consolaId) 
        { 
            return _juegoRepository.ObtenerPorConsola(consolaId); 
        }

        public List<Juego> ObtenerJuegosDestacados()
        {
            return _juegoRepository.ObtenerDestacados();
        }

        public List<Consola> ObtenerConsolas() 
        { 
            return _consolaRepository.ObtenerTodas(); 
        }

        public List<Consola> ObtenerConsolasActivas() 
        { 
            return _consolaRepository.ObtenerActivas(); 
        }

        public Consola ObtenerConsolaPorId(int id)
        {
            return _consolaRepository.ObtenerPorId(id);
        }
    }
}
