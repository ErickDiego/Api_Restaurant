using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Repository.Repositorios
{
    public class UsuarioJsonDTO
    {
        public string rut { get; set; }
        public int idPerfil { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public int activo { get; set; }
        public string nombreUsuario { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string contrasena { get; set; }
    }
}
