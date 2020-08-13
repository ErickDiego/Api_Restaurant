using System;
using System.Collections.Generic;

namespace Restaurant.Data
{
    public partial class Detalle
    {
        public int IdDetalle { get; set; }
        public int IdPedido { get; set; }
        public int IdSeleccion { get; set; }
        public int Cantidad { get; set; }

        public virtual Pedido IdPedidoNavigation { get; set; }
        public virtual Seleccion IdSeleccionNavigation { get; set; }
    }
}
