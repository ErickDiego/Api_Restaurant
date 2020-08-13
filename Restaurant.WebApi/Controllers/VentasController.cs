using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data;

namespace Restaurant.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly RestaurantBDContext _context;

        public VentasController(RestaurantBDContext context)
        {
            _context = context;
        }

        // GET: api/Ventas
        [Route("[Action]")]
        [HttpGet]
        public async Task<ActionResult> ObtenerVentas()
        {
            try
            {
                return Ok(new
                {
                    resultado = 200,
                    Total = await _context.Ventas.CountAsync(),
                    Mensaje = "Se ha obtenido los Ventas correctamente",
                    lista = await _context.Ventas.ToListAsync()
                });
            }
            catch (Exception ex)
            {

                return Ok(new
                {
                    resultado = 400,
                    mensaje = "A ocurrido un error, exepcion: " + ex.Message

                });
            }
        }

        // GET: api/Ventas/5

        [Route("[Action]/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult> ObtenerVenta(int id)
        {
            try
            {
                var venta = await _context.Ventas.FindAsync(id);    

                return Ok(new
                {
                    resultado = 200,
                    mensaje = "Mesa obtenida exitosamente",
                    Venta = venta

                });

            }
            catch (Exception ex)
            {

                if (_context.Ventas.Any(e => e.IdVenta == id) == false)
                {
                    return Ok(new
                    {
                        resultado = 404,
                        mensaje = "No se a encontrado la Venta con Id: " + id
                    });
                }
                return Ok(new
                {
                    resultado = 400,
                    mensaje = "A ocurrido un problema, exepcion: " + ex.Message
                });
            }

        }

        // PUT: api/Ventas/5
        [Route("[Action]/{id}")]
        [HttpPost("{id}")]
        public async Task<IActionResult> EditarVenta(int id, Venta venta)
        {
            if (id != venta.IdVenta)
            {
                return Ok();
            }

            _context.Entry(venta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VentaExists(id))
                {
                    return Ok();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Ventas
        [Route("[Action]")]
        [HttpPost]
        public async Task<ActionResult> AgregarVenta(Venta venta)
        {
            try
            {
                venta.FechaVenta = DateTime.Now;

                _context.Ventas.Add(venta);

                await _context.SaveChangesAsync();
                return Ok(new
                {
                    Resultado = 200,
                    Mensaje = "Se a almacenado correctamente la Venta"
                });
            }
            catch (Exception ex)
            {


                return Ok(new
                {
                    Resultado = 400,
                    Mensaje = "A ocurrido un problema, exepcion: " + ex.Message
                });

            }
        }

        // DELETE: api/Ventas/5
        [Route("[Action]/{id}")]
        [HttpPost("{id}")]
        public async Task<ActionResult<Venta>> EliminarVenta(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
            {
                return Ok();
            }

            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();

            return venta;
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.IdVenta == id);
        }
    }
}
