using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoplaElCartucho.Business.Services;

namespace SoplaElCartucho.Tests
{
    [TestClass]
    public class CatalogoServiceTests
    {
        private CatalogoService _catalogoService;

        [TestInitialize]
        public void Setup()
        {
            _catalogoService = new CatalogoService();
        }

        [TestMethod]
        public void ObtenerJuegos_DebeRetornarLista()
        {
            var result = _catalogoService.ObtenerJuegos();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ObtenerConsolas_DebeRetornarLista()
        {
            var result = _catalogoService.ObtenerConsolas();
            Assert.IsNotNull(result);
        }
    }
}
