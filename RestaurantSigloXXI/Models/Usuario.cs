using System;
using System.Collections.Generic;

namespace RestaurantSigloXXI.Models
{
    public partial class Usuario
    {
        public int Rut { get; set; }
        public int IdPerfil { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int Activo { get; set; }

        public virtual Perfil IdPerfilNavigation { get; set; }
    }
}
