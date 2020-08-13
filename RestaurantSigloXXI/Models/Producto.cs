using System;
using System.Collections.Generic;

namespace RestaurantSigloXXI.Models
{
    public partial class Producto
    {
        public Producto()
        {
            SeleccionProducto = new HashSet<SeleccionProducto>();
        }

        public int IdProducto { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<SeleccionProducto> SeleccionProducto { get; set; }
    }
}
