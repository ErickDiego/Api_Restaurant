using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
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
    public class SeleccionesController : ControllerBase
    {
        private readonly RestaurantBDContext _context;

        public SeleccionesController(RestaurantBDContext context)
        {
            _context = context;
        }

        // GET: api/Seleccions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seleccion>>> GetSelecciones()
        {
            return await _context.Selecciones.ToListAsync();
        }

        // GET: api/Seleccions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Seleccion>> GetSeleccion(int id)
        {
            var seleccion = await _context.Selecciones.FindAsync(id);

            if (seleccion == null)
            {
                return Ok();
            }

            return seleccion;
        }

        // PUT: api/Seleccions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarSeleccion(int id, Seleccion seleccion)
        {
            if (id != seleccion.IdSeleccion)
            {
                return Ok();
            }

            _context.Entry(seleccion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeleccionExists(id))
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

        // POST: api/Seleccions
        [HttpPost]
        public async Task<ActionResult<Seleccion>> PostSeleccion(Seleccion seleccion)
        {
            _context.Selecciones.Add(seleccion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSeleccion", new { id = seleccion.IdSeleccion }, seleccion);
        }

        // DELETE: api/Seleccions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Seleccion>> DeleteSeleccion(int id)
        {
            var seleccion = await _context.Selecciones.FindAsync(id);
            if (seleccion == null)
            {
                return Ok();
            }

            _context.Selecciones.Remove(seleccion);
            await _context.SaveChangesAsync();

            return seleccion;
        }

        private bool SeleccionExists(int id)
        {
            return _context.Selecciones.Any(e => e.IdSeleccion == id);
        }

        [Route("[Action]/{pagina}")]
        [HttpGet("{pagina}")]
        public async Task<ActionResult> ListadoDeMenu(int pagina)
        {
            List<object> listaSeleccion = new List<object>();
            double CantidadTotal = await _context.Selecciones.CountAsync();
            try
            {
                using (SqlConnection conn = (SqlConnection)_context.Database.GetDbConnection())
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "ListadoMenu";
                        cmd.Parameters.AddWithValue("@PageNumber", pagina);

                        DbDataReader oReader = await cmd.ExecuteReaderAsync();

                        if (oReader.HasRows)
                        {
                            while (await oReader.ReadAsync())
                            {
                                var row = new
                                {
                                    IdSeleccion = oReader.GetInt32(0),
                                    Nombre = oReader.GetString(1),
                                    Valor = oReader.GetInt32(2),
                                    TIEMPO = oReader.GetInt32(3),
                                    IMAGEN = oReader.GetString(4),
                                };
                                listaSeleccion.Add(row);
                            }
                        }

                        oReader.Dispose();
                    }
                }
                return Ok(new
                {
                    Total = Convert.ToInt32(CantidadTotal),
                    Cant_paginas = Convert.ToInt32(Math.Ceiling(CantidadTotal / 10)),
                    Resultado = 200,
                    Mensaje = "Selecciones devueltas correctamente",
                    Lista = listaSeleccion
                });

            }
            catch (Exception ex)
            {
                //throw;
                return Ok(
                    new
                    {
                        resultado = 400,
                        mensaje = "A ocurrido un problema, exepcion: " + ex.Message
                    }
                    );

            }
        }

    }
}

