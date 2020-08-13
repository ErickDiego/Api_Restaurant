using System;
using System.Collections.Generic;

namespace Restaurant.Data
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
        public int Ubicacion { get; set; }
        public int Capacidad { get; set; }
        public int Estado { get; set; }

        public virtual EstadoMesa EstadoNavigation { get; set; }
        public virtual Ubicacion UbicacionNavigation { get; set; }
        public virtual ICollection<Pedido> Pedido { get; set; }
        public virtual ICollection<Reserva> Reserva { get; set; }
        public virtual ICollection<Venta> Venta { get; set; }
    }
}
