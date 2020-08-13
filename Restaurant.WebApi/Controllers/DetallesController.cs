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

namespace Restaurant.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallesController : ControllerBase
    {
        private readonly RestaurantBDContext _context;

        public DetallesController(RestaurantBDContext context)
        {
            _context = context;
        }

        // GET: api/Detalles
        [Route("[Action]")]
        [HttpGet]
        public async Task<ActionResult> ObtenerDetalles()
        {
            try
            {
                List<Detalle> detalles = await _context.Detalles.ToListAsync();
                return Ok(new
                {
                    resultado = 200,
                    Mensaje = "Se han obtenido correctamente los detalles",
                    Lista = detalles

                });
            }
            catch (Exception ex)
            {

                return Ok(new
                {
                    resultado = 400,
                    Mensaje = "A ocurrido un problema, expecion: " + ex.Message

                });
            }
        }

        // GET: api/Detalles/5
        [Route("[Action]/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult> ObtenerDetalle(int id)
        {
            try
            {
                var detalle = await _context.Detalles.FindAsync(id);

                if (detalle == null)
                {
                    return Ok();
                }

                return Ok();
            }
            catch (Exception)
            {

                return Ok();
            }
        }

        // PUT: api/Detalles/5
        [Route("[Action]/{id}")]
        [HttpPost("{id}")]
        public async Task<ActionResult> EditarDetalle(int id, Detalle detalle)
        {
            if (id != detalle.IdDetalle)
            {
                return Ok();
            }

            _context.Entry(detalle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetalleExists(id))
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

        // POST: api/Detalles
        [Route("[Action]")]
        [HttpPost]
        public async Task<ActionResult> AgregarDetalle(List<Detalle> Listadetalle)
        {
            try
            {
                foreach (var item in Listadetalle)
                {
                    _context.Detalles.Add(item);
                    await _context.SaveChangesAsync();

                    DescontarStockProducto(item.IdSeleccion);
                }

                return Ok(new
                {
                    Resultado = 200,
                    Mensaje = "Se han almacenado correctamente los detalles."
                });

            }
            catch (DbUpdateException ex)
            {
                return Ok(new
                {
                    Resultado = 400,
                    Mensaje = "A ocurrido un problema, exepcion: " + ex.Message
                });
            }
        }

        // DELETE: api/Detalles/5
        [Route("[Action]/{id}")]
        [HttpPost("{id}")]
        public async Task<ActionResult> EliminarDetalle(int id)
        {
            try
            {
                var detalle = await _context.Detalles.FindAsync(id);
                if (detalle == null)
                {
                    return Ok();
                }

                _context.Detalles.Remove(detalle);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {

                return Ok();
            }
        }

        private bool DetalleExists(int id)
        {
            return _context.Detalles.Any(e => e.IdDetalle == id);
        }

        private async void DescontarStockProducto(int IdSelccion)
        {
            try
            {
                using (SqlConnection conn = (SqlConnection)_context.Database.GetDbConnection())
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "DisminuirStock";
                        cmd.Parameters.AddWithValue("@idselccion", IdSelccion);

                        DbDataReader oReader = await cmd.ExecuteReaderAsync();

                        oReader.Dispose();
                    }
                }


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
