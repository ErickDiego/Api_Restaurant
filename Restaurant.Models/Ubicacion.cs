using System;
using System.Collections.Generic;

namespace Restaurant.Data
{
    public partial class Ubicacion
    {
        public Ubicacion()
        {
            Mesa = new HashSet<Mesa>();
        }

        public int IdUbicacion { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Mesa> Mesa { get; set; }
    }
}
