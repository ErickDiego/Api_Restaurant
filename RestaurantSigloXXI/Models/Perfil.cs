using System;
using System.Collections.Generic;

namespace RestaurantSigloXXI.Models
{
    public partial class Perfil
    {
        public Perfil()
        {
            Usuario = new HashSet<Usuario>();
        }

        public int IdPerfil { get; set; }
        public int IdModulo { get; set; }
        public string NombrePefil { get; set; }

        public virtual Modulo IdModuloNavigation { get; set; }
        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
