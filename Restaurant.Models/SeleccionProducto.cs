using System;
using System.Collections.Generic;

namespace Restaurant.Data
{
    public partial class SeleccionProducto
    {
        public int IdSeleccionProducto { get; set; }
        public int IdSeleccion { get; set; }
        public int IdProducto { get; set; }
        public int Unidades { get; set; }

        public virtual Producto IdProductoNavigation { get; set; }
        public virtual Seleccion IdSeleccionNavigation { get; set; }
    }
}
