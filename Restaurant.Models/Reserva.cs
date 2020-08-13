using System;
using System.Collections.Generic;

namespace Restaurant.Data
{
    public partial class Reserva
    {
        public int IdReserva { get; set; }
        public int IdMesa { get; set; }
        public int? RutCliente { get; set; }
        public int CantidadPersonas { get; set; }
        public DateTime? Fecha { get; set; }

        public virtual Mesa IdMesaNavigation { get; set; }
        public virtual Cliente RutClienteNavigation { get; set; }
    }
}
