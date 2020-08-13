using System;
using System.Collections.Generic;

namespace Restaurant.Data
{
    public partial class Pedido
    {
        public Pedido()
        {
            Detalle = new HashSet<Detalle>();
        }

        public int IdPedido { get; set; }
        public int IdMesa { get; set; }
        public int IdEstadoPedido { get; set; }

        public virtual Estado IdEstadoPedidoNavigation { get; set; }
        public virtual Mesa IdMesaNavigation { get; set; }
        public virtual ICollection<Detalle> Detalle { get; set; }
    }
}
