using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace RestaurantSigloXXI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        
        // POST: api/Pedido
        [HttpPost]
        public CreatedAtRouteResult obtenerPedidoPost([FromBody] string value)
        {
            using (Models.RestaurantBDContext bd = new Models.RestaurantBDContext())
            {
                string json = "";
                var query = from b in bd.Pedido
                            select b;
                foreach (var item in query)
                {

                    json = JsonConvert.SerializeObject(item);
                }
                return new CreatedAtRouteResult("GetPedido", json);


            }
        }

  
    }
}
