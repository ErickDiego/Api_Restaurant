using System;
using System.Collections.Generic;

namespace RestaurantSigloXXI.Models
{
    public partial class Venta
    {
        public int IdVenta { get; set; }
        public int IdMesa { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaVenta { get; set; }
        public int TotalAPagar { get; set; }
        public string MetodoPago { get; set; }

        public virtual Mesa IdMesaNavigation { get; set; }
    }
}
