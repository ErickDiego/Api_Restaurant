using System;
using System.Collections.Generic;

namespace RestaurantSigloXXI.Models
{
    public partial class Mesa
    {
        public Mesa()
        {
            Pedido = new HashSet<Pedido>();
            Reserva = new HashSet<Reserva>();
            Venta = new HashSet<Venta>();
        }

        public int IdMesa { get; set; }
        public string Ubicacion { get; set; }
        public int Capacidad { get; set; }
        public int IdEstadoMesa { get; set; }

        public virtual ICollection<Pedido> Pedido { get; set; }
        public virtual ICollection<Reserva> Reserva { get; set; }
        public virtual ICollection<Venta> Venta { get; set; }
    }
}
