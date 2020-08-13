using Microsoft.VisualStudio.TestTools.UnitTesting;
using Restaurant.Repository.Contratos;
using Restaurant.Repository.Repositorios;
using System;
using System.Net.Http;

namespace PruebasUnitarias
{
    [TestClass]
    public class TestUsuario
    {
        private IUsuarioRepositorio _usuarioRepositorio;

        public TestUsuario(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        [TestMethod]
        public void ValidarTest()
        {

            //_usuarioRepositorio.validar(15389125, "1234");

            //Assert.IsNotNull(_usuarioRepositorio.validar(15389125, "1234"));

        }
    }
}
