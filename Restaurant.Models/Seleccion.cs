using System;
using System.Collections.Generic;

namespace Restaurant.Data
{
    public partial class Seleccion
    {
        public Seleccion()
        {
            Detalle = new HashSet<Detalle>();
            SeleccionProducto = new HashSet<SeleccionProducto>();
        }

        public int IdSeleccion { get; set; }
        public string Nombre { get; set; }
        public int Valor { get; set; }
        public int? Tiempo { get; set; }
        public string Imagen { get; set; }

        public virtual ICollection<Detalle> Detalle { get; set; }
        public virtual ICollection<SeleccionProducto> SeleccionProducto { get; set; }
    }
}
