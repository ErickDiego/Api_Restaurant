using System;
using System.Collections.Generic;

namespace RestaurantSigloXXI.Models
{
    public partial class Detalle
    {
        public int IdDetalle { get; set; }
        public int IdPedido { get; set; }
        public int IdSeleccion { get; set; }
        public int Cantidad { get; set; }

        public virtual Pedido IdPedidoNavigation { get; set; }
    }
}
