using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoplaElCartucho.Business.Services;

namespace SoplaElCartucho.Tests
{
    [TestClass]
    public class CarritoServiceTests
    {
        private CarritoService _carritoService;

        [TestInitialize]
        public void Setup()
        {
            _carritoService = new CarritoService();
        }

        [TestMethod]
        public void ObtenerItems_DebeRetornarLista()
        {
            var result = _carritoService.ObtenerItems("test-session");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CalcularTotal_SinItems_DebeRetornarCero()
        {
            var result = _carritoService.CalcularTotal("test-session");
            Assert.AreEqual(0m, result);
        }
    }
}
