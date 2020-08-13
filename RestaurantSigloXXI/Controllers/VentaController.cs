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
    public class VentaController : ControllerBase
    {
       
        // POST: api/Venta
        [HttpPost]
        public CreatedAtRouteResult obtenerVentaPost([FromBody] string value)
        {
            using (Models.RestaurantBDContext bd = new Models.RestaurantBDContext())
            {
                string json = "";
                var query = from b in bd.Venta
                            select b;
                foreach (var item in query)
                {

                    json = JsonConvert.SerializeObject(item);
                }
                return new CreatedAtRouteResult("GetVenta", json);


            }
        }

      
    }
}
