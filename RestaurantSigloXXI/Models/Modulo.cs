using System;
using System.Collections.Generic;

namespace RestaurantSigloXXI.Models
{
    public partial class Modulo
    {
        public Modulo()
        {
            Perfil = new HashSet<Perfil>();
        }

        public int IdModulo { get; set; }
        public string NombreModulo { get; set; }

        public virtual ICollection<Perfil> Perfil { get; set; }
    }
}
