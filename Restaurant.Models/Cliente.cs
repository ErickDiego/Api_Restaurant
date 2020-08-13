using System;
using System.Collections.Generic;

namespace Restaurant.Data
{
    public partial class Cliente
    {
        public Cliente()
        {
            Reserva = new HashSet<Reserva>();
        }

        public int Rut { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Reserva> Reserva { get; set; }
    }
}
