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
    public class ClientesController : ControllerBase
    {
        private readonly RestaurantBDContext _context;

        public ClientesController(RestaurantBDContext context)
        {
            _context = context;
        }

        // GET: api/Clientes
        [Route("[Action]")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        // GET: api/Clientes/5
        [Route("[Action]/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return Ok();
            }

            return cliente;
        }

        // PUT: api/Clientes/5
        [HttpPost("{id}")]
        public async Task<IActionResult> ActualizarCliente(int id, Cliente cliente)
        {
            if (id != cliente.Rut)
            {
                return Ok();
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
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

        // POST: api/Clientes
        [HttpPost]
        public async Task<ActionResult<Cliente>> IngresarCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClienteExists(cliente.Rut))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCliente", new { id = cliente.Rut }, cliente);
        }

        // DELETE: api/Clientes/5
        [Route("[Action]/{id}")]
        [HttpPost("{id}")]
        public async Task<ActionResult<Cliente>> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return Ok();
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return cliente;
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Rut == id);
        }
    }
}
