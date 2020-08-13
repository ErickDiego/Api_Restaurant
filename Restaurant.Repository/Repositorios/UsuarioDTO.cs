using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Repository.Repositorios
{
    public class UsuarioDTO
    {
        public int Rut { get; set; }
        public int IdPerfil { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int Activo { get; set; }
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }

        public UsuarioDTO()
        {

        }
    }
}
