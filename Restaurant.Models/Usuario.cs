using System;
using System.Collections.Generic;

namespace Restaurant.Data
{
    public partial class Usuario
    {
        public int Rut { get; set; }
        public int IdPerfil { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int Activo { get; set; }
        public string Contrasena { get; set; }
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }

        public virtual Perfil IdPerfilNavigation { get; set; }
    }
}
