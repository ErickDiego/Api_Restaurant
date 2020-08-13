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
    public class EstadoController : ControllerBase
    {
        // GET: api/Estado
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Estado/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Estado
        [HttpPost]
        public CreatedAtRouteResult obtenerEstadoPost([FromBody] string value)
        {
            using (Models.RestaurantBDContext bd = new Models.RestaurantBDContext())
            {
                string json = "";
                var query = from b in bd.Estado
                            select b;
                foreach (var item in query)
                {

                    json = JsonConvert.SerializeObject(item);
                }
                return new CreatedAtRouteResult("prueba existosa", json);


            }
        }


    }
}
