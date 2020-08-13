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
    public class ClienteController : ControllerBase
    {
        // GET: api/Cliente
       // [HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/Cliente/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Cliente
        [HttpPost]
        public CreatedAtRouteResult Post([FromBody] string value)
        {
            using (Models.RestaurantBDContext bd = new Models.RestaurantBDContext())
            {
                string json = "";
                var query = from b in bd.Cliente
                            select b;
                foreach (var item in query)
                {

                    json = JsonConvert.SerializeObject(item);
                }
                return new CreatedAtRouteResult("prueba existosa", json);


            }
        }

        // PUT: api/Cliente/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
