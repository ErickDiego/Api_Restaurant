using System;
using System.Collections.Generic;

namespace Restaurant.Data
{
    public partial class EstadoMesa
    {
        public EstadoMesa()
        {
            Mesa = new HashSet<Mesa>();
        }

        public int IdEstado { get; set; }
        public string DescripcionEstado { get; set; }

        public virtual ICollection<Mesa> Mesa { get; set; }
    }
}
