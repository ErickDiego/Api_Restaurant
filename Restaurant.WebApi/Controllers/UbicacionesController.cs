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
    public class UbicacionesController : ControllerBase
    {
        private readonly RestaurantBDContext _context;

        public UbicacionesController(RestaurantBDContext context)
        {
            _context = context;
        }

        // GET: api/Ubicaciones/ObtenerUbicaciones
        [Route("[Action]")]
        [HttpGet]
        public async Task<ActionResult> ObtenerUbicaciones()
        {

            try
            {
                return Ok(new
                {
                    resultado = 200,
                    Total = await _context.Ubicaciones.CountAsync(),
                    Mensaje = "Se han obtenido las ubicaciones correctamente",
                    lista = await _context.Ubicaciones.Select(u => new { u.IdUbicacion, NombreUbicacion = u.Nombre }).ToListAsync()
                });
            }
            catch (Exception ex)
            {

                return Ok(new
                {
                    resultado = 400,
                    mensaje = "A ocurrido un error al obtener las ubicaciones, exepcion: " + ex.Message

                });
            }
        }

        // GET: api/Ubicaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ubicacion>> GetUbicacion(int id)
        {
            var ubicacion = await _context.Ubicaciones.FindAsync(id);

            if (ubicacion == null)
            {
                return Ok();
            }

            return ubicacion;
        }

        // PUT: api/Ubicaciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUbicacion(int id, Ubicacion ubicacion)
        {
            if (id != ubicacion.IdUbicacion)
            {
                return Ok();
            }

            _context.Entry(ubicacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UbicacionExists(id))
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

        // POST: api/Ubicaciones
        [HttpPost]
        public async Task<ActionResult<Ubicacion>> PostUbicacion(Ubicacion ubicacion)
        {
            _context.Ubicaciones.Add(ubicacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUbicacion", new { id = ubicacion.IdUbicacion }, ubicacion);
        }

        // DELETE: api/Ubicaciones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Ubicacion>> DeleteUbicacion(int id)
        {
            var ubicacion = await _context.Ubicaciones.FindAsync(id);
            if (ubicacion == null)
            {
                return Ok();
            }

            _context.Ubicaciones.Remove(ubicacion);
            await _context.SaveChangesAsync();

            return ubicacion;
        }

        private bool UbicacionExists(int id)
        {
            return _context.Ubicaciones.Any(e => e.IdUbicacion == id);
        }
    }
}
