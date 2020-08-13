using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Models;

namespace Restaurant.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuloesController : ControllerBase
    {
        private readonly RestaurantBDContext _context;

        public ModuloesController(RestaurantBDContext context)
        {
            _context = context;
        }

        // GET: api/Moduloes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Modulo>>> GetModulos()
        {
            return await _context.Modulos.ToListAsync();
        }

        // GET: api/Moduloes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Modulo>> GetModulo(int id)
        {
            var modulo = await _context.Modulos.FindAsync(id);

            if (modulo == null)
            {
                return Ok();
            }

            return modulo;
        }

        // PUT: api/Moduloes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModulo(int id, Modulo modulo)
        {
            if (id != modulo.IdModulo)
            {
                return Ok();
            }

            _context.Entry(modulo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuloExists(id))
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

        // POST: api/Moduloes
        [HttpPost]
        public async Task<ActionResult<Modulo>> PostModulo(Modulo modulo)
        {
            _context.Modulos.Add(modulo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModulo", new { id = modulo.IdModulo }, modulo);
        }

        // DELETE: api/Moduloes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Modulo>> DeleteModulo(int id)
        {
            var modulo = await _context.Modulos.FindAsync(id);
            if (modulo == null)
            {
                return Ok();
            }

            _context.Modulos.Remove(modulo);
            await _context.SaveChangesAsync();

            return modulo;
        }

        private bool ModuloExists(int id)
        {
            return _context.Modulos.Any(e => e.IdModulo == id);
        }
    }
}
