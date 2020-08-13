using System;
using System.Collections.Generic;

namespace RestaurantSigloXXI.Models
{
    public partial class Seleccion
    {
        public int IdSeleccion { get; set; }
        public string Nombre { get; set; }
        public int Valor { get; set; }

        public virtual SeleccionProducto IdSeleccionNavigation { get; set; }
    }
}
