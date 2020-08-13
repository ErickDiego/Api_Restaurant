using System;
using System.Collections.Generic;

namespace RestaurantSigloXXI.Models
{
    public partial class SeleccionProducto
    {
        public int IdSeleccion { get; set; }
        public int IdProducto { get; set; }
        public int Unidades { get; set; }

        public virtual Producto IdProductoNavigation { get; set; }
        public virtual Seleccion Seleccion { get; set; }
    }
}
