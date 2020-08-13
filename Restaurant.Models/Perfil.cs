using System;
using System.Collections.Generic;

namespace Restaurant.Data
{
    public partial class Perfil
    {
        public Perfil()
        {
            Usuario = new HashSet<Usuario>();
        }

        public int IdPerfil { get; set; }
        public string NombrePefil { get; set; }

        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
