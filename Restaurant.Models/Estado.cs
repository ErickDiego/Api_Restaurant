using System;
using System.Collections.Generic;

namespace Restaurant.Data
{
    public partial class Estado
    {
        public Estado()
        {
            Pedido = new HashSet<Pedido>();
        }

        public int IdEstado { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Pedido> Pedido { get; set; }
    }
}
