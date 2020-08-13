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
    public class ReservasController : ControllerBase
    {
        private readonly RestaurantBDContext _context;

        public ReservasController(RestaurantBDContext context)
        {
            _context = context;
        }


        // GET: api/Reservas/ObtenerReservas
        [Route("[Action]")]
        [HttpGet]
        public async Task<ActionResult> ObtenerReservas()
        {
            try
            {
                List<Reserva> ListaReserva = new List<Reserva>();

                var obj = await _context.Reservas.ToListAsync();

                foreach (var item in obj)
                {
                    Reserva reserva = new Reserva();

                    reserva.IdReserva = item.IdReserva;
                    reserva.IdMesa = item.IdMesa;
                    reserva.RutCliente = item.RutCliente;
                    reserva.CantidadPersonas = item.CantidadPersonas;
                    reserva.Fecha = item.Fecha;

                    ListaReserva.Add(reserva);

                }

                return Ok(new
                                {
                                    Resultado = 200,
                                    Mensaje = "Se han obtenido correctamente el Listado de las Reservas",
                                    Lista = ListaReserva
                                }
                        );

            }catch (Exception ex)
            {
              //  var result = ;
                if (await _context.Reservas.CountAsync() > 0)
                {
                    NotFound(new
                    {
                        Resultado = 404,
                        Mensaje = "No se han encontrado reservas, expecion: " + ex.Message
                    });
                }
                else
                {
                    return Ok(new
                    {
                        Resultado = 400,
                        Mensaje = "Se ha producido un problema al obtener las reservas, expecion: " + ex.Message
                    });
                }
            }
            return Ok();
        }

        // GET: api/Reservas/5
        [Route("[Action]/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> ObtenerReserva(int id)
        {
            try
            {
                var reserva = await _context.Reservas.FindAsync(id);

                return Ok(new
                {
                    resultado = 200,
                    mensaje = "Se ha almacenado correctamente la reserva"
                });
            }
            catch (Exception ex)
            {

                if (await _context.Reservas.FindAsync(id) == null)
                {
                    return Ok(new
                    {
                        resultado = 404,
                        mensaje = "No se a podido encontrar la Reserva, Exepcion: " + ex.Message
                    });
                }
                else
                {
                    return Ok(new
                    {
                        resultado = 400,
                        mensaje = "A ocurrido un problema en la busqueda de la Reserva, Exepcion: " + ex.Message
                    });
                }

            }

        }

        // PUT: api/Reservas/5
        [Route("[Action]/{id}")]
        [HttpPost("{id}")]
        public async Task<IActionResult> EditarReserva(int id, Reserva reserva)
        {
            if (id != reserva.IdReserva)
            {
                return Ok(new
                {
                    resultado = 400,
                    mensaje = id + " no es igual a la reserva de ID: " + reserva.IdReserva
                });
            }

            _context.Entry(reserva).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    resultado = 200,
                    Mensaje = "Reserva Editada Correctamente"
                });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ReservaExists(id))
                {
                    return Ok(new
                    {
                        resultado = 404,
                        mensaje = "No se a encontrado la Reserva con id: " + id.ToString()
                    });
                }
                else
                {
                    return Ok(new
                    {
                        resultado = 400,
                        mensaje = "A ocurrido un error, Exepcion: " + ex.Message
                    });
                }
            }

        }

        // POST: api/Reservas/AgregarReserva
        [Route("[Action]")]
        [HttpPost]
        public async Task<ActionResult> AgregarReserva(Reserva reserva)
        {
            try
            {
                reserva.Fecha = DateTime.Now;
                    _context.Reservas.Add(reserva);
                    await _context.SaveChangesAsync();

                return Ok(new
                {
                    resultado = 200,
                    mensaje = "La reserva se realizado exitosamente"
                });

            }
            catch (DbUpdateException ex)
            {
                if (ReservaExists(reserva.IdReserva))
                {
                    return Ok(new
                    {
                        resultado = 400,
                        mensaje = "Reserva ya se encuentra Registrado"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        resultado = 400,
                        mensaje = ex.Message
                    });

                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    resultado = 400,
                    mensaje = "Error inesperado: " + ex.Message
                });
            }


            
        }

        // DELETE: api/Reservas/EliminarReserva/5
        [Route("[Action]/{id}")]
        [HttpPost("{id}")]
        public async Task<ActionResult> EliminarReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return Ok(new
                {
                    resultado = 400,
                    mensaje = "No se pudo Eliminar la Reserva"
                });
            }

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            //return reserva;
            return Ok(new
            {
                resultado = 200,
                mensaje = "Reserva eliminada exitosamente"
            });
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.IdReserva == id);
        }


        [Route("[Action]/{rut}")]
        [HttpPost("{rut}")]
        public async Task<ActionResult> BuscarReservaPorCliente(int rut)
        {
            try
            {
                List<Reserva> ListaReserva = await _context.Reservas.Where(r => r.RutCliente == rut).ToListAsync();


                return Ok(new
                {
                    resultado = 200,
                    mensaje = "Cliente Posee Reserva",
                    Lista = ListaReserva
                });
            }
            catch (Exception ex)
            {

                return Ok(new
                {
                    resultado = 400,
                    Mensaje = "A ocurrido un problema en la busqueda de las reservas, exepcion: " + ex.Message
                });
            }
        }
    }
}
