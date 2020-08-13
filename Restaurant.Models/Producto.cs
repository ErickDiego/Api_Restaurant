using System;
using System.Collections.Generic;

namespace Restaurant.Data
{
    public partial class Producto
    {
        public Producto()
        {
            SeleccionProducto = new HashSet<SeleccionProducto>();
        }

        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public int Stock { get; set; }
        public DateTime FechaUltimaReposicion { get; set; }
        public int CantidadRecomendada { get; set; }

        public virtual ICollection<SeleccionProducto> SeleccionProducto { get; set; }
    }
}
