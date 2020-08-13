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
    public class MesasController : ControllerBase
    {
        private readonly RestaurantBDContext _context;

        public MesasController(RestaurantBDContext context)
        {
            _context = context;
        }

        // GET: api/Mesas/ObtenerMesas
        [Route("[Action]")]
        [HttpGet]
        public async Task<ActionResult> ObtenerMesas()
        {
            //return await _context.Mesas.ToListAsync();
            try
            {
                return Ok(new
                {
                    resultado = 200,
                    Total = await _context.Mesas.CountAsync(),
                    Mensaje = "Se han obtenido las mesas correctamente",
                    lista = await _context.Mesas.ToListAsync()
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

        //GET: api/Mesas/ObtenerMesas/1
        [Route("[Action]/{nPaginas}")]
        [HttpGet("{nPaginas}")]
        public async Task<ActionResult> ObtenerMesas(int nPaginas = 1, int nRegistros = 10)
        {

            try
            {
                var consultaMesas = _context.Mesas.AsQueryable();
                var totalMesas = consultaMesas.Count();
                var totalPaginas = ((int)Math.Ceiling((double)totalMesas / nRegistros));

                var datosPaginados = await consultaMesas
                    .Skip(nRegistros * (nPaginas - 1))
                    .Take(nRegistros)
                    .Select(m => new
                    {
                        m.IdMesa,
                        m.EstadoNavigation.IdEstado,
                        m.UbicacionNavigation.IdUbicacion,
                        m.Capacidad,
                        m.EstadoNavigation.DescripcionEstado,
                        m.UbicacionNavigation.Nombre
                    })
                    .ToListAsync();



                return Ok(new
                {
                    resultado = 200,
                    total = totalMesas,
                    nPaginas = totalPaginas,
                    mensaje = "Se han obtenido las mesas correctamente",
                    lista = datosPaginados
                }); ;
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

        // GET: api/Mesas/5
        [Route("[Action]/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult> ObtenerMesa(int id)
        {
            try
            {
                var mesa = await _context.Mesas.FindAsync(id);
                EstadoMesa estadoMesa = await _context.EstadoMesas.Where(d => d.IdEstado == mesa.Estado).FirstAsync();


                return Ok(new
                {
                    resultado = 200,
                    mensaje = "Mesa obtenida exitosamente",
                    Mesa = new
                    {
                        mesa.IdMesa,
                        mesa.Ubicacion,
                        mesa.Capacidad,
                        mesa.Estado,
                        estadoMesa.DescripcionEstado
                    }

                });

            }
            catch (Exception ex)
            {

                if (_context.Mesas.Any(e => e.IdMesa == id) == false)
                {
                    return Ok(new
                    {
                        resultado = 404,
                        mensaje = "No se a encontrado la mesa con Id: " + id
                    });
                }
                return Ok(new
                {
                    resultado = 400,
                    mensaje = "A ocurrido un problema, exepcion: " + ex.Message
                });
            }

        }

        // PUT: api/Mesas/5
        [Route("[Action]/{id}")]
        [HttpPost("{id}")]
        public async Task<ActionResult> EditarMesa(int id, Mesa mesa)
        {
            if (id != mesa.IdMesa)
            {
                return Ok(new
                {
                    resultado = 400,
                    mensaje = "Hubo un problema al editar la Mesa"
                });
            }


            try
            {
                Mesa mesa1 = await _context.Mesas.FirstOrDefaultAsync(u => u.IdMesa == mesa.IdMesa);
                //EstadoMesa estadoMesa = await _context.EstadoMesas.Where(d => d.IdEstado == mesa.Estado).FirstAsync();

                mesa1.IdMesa = mesa1.IdMesa;
                mesa1.Ubicacion = mesa.Ubicacion == 0 ? mesa1.Ubicacion : mesa.Ubicacion; //condition ? consequent : alternative
                mesa1.Capacidad = mesa.Capacidad == 0 ? mesa1.Capacidad : mesa.Capacidad; //condition ? consequent : alternative
                mesa1.Estado = mesa.Estado == 0 ? mesa1.Estado : mesa.Estado; //condition ? consequent : alternative
                //mesa1.DescripcionEstado = mesa.DescripcionEstado == "" ? mesa1.DescripcionEstado : mesa.DescripcionEstado; //condition ? consequent : alternative

                _context.Entry(mesa1).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    resultado = 200,
                    mensaje = "Mesas Editada exitosamente"
                });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!MesaExists(id))
                {
                    return Ok(new
                    {
                        resultado = 404,
                        mensaje = "No existe mesa asociada al ID: " + id
                    });
                }
                else
                {
                    return Ok(new
                    {
                        resultado = 400,
                        mensaje = "A ocurrido un problema, exepcion: " + ex.Message
                    });
                }
            }
        }

        // POST: api/Mesas
        [Route("[Action]")]
        [HttpPost]
        public async Task<ActionResult> AgregarMesa(Mesa mesa)
        {
            try
            {
                _context.Mesas.Add(mesa);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    resultado = 200,
                    mensaje = "Mesa guardada exitosamente"
                });
            }
            catch (DbUpdateException ex)
            {
                if (MesaExists(mesa.IdMesa))
                {
                    return Conflict(new
                    {
                        resultado = 409,
                        mensaje = "Mesa ya se encuentra Registrado"
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

        }

        // DELETE: api/Mesas/5
        [Route("[Action]/{id}")]
        [HttpPost("{id}")]
        public async Task<ActionResult> EliminarMesa(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa == null)
            {
                return Ok(new
                {
                    resultado = 404,
                    mensaje = "No se pudo Eliminar el Mesa, no fue encontrada"
                });
            }

            _context.Mesas.Remove(mesa);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                resultado = 200,
                mensaje = "Mesa eliminada exitosamente"
            });
        }

        private bool MesaExists(int id)
        {
            return _context.Mesas.Any(e => e.IdMesa == id);
        }

        [Route("[Action]/{pagina}")]
        [HttpGet("{pagina}")]
        public async Task<ActionResult> ListadoDeMesas(int pagina)
        {
            List<Object> listaMesas = new List<Object>();
            double CantidadTotal = await _context.Mesas.CountAsync();
            try
            {
                using (SqlConnection conn = (SqlConnection)_context.Database.GetDbConnection())
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "ListadoMesas";
                        cmd.Parameters.AddWithValue("@PageNumber", pagina);

                        DbDataReader oReader = await cmd.ExecuteReaderAsync();

                        if (oReader.HasRows)
                        {
                            while (await oReader.ReadAsync())
                            {
                                var row = new // Mesa
                                {
                                    IdMesa = oReader.GetInt32(0),
                                    Ubicacion = oReader.GetInt32(1),
                                    Capacidad = oReader.GetInt32(2),
                                    Estado = oReader.GetInt32(3),
                                    DescripcionEstado = String.IsNullOrEmpty(oReader.GetString(4)) == false ? oReader.GetString(4) : "", //mesa.DescripcionEstado == "" ? mesa1.DescripcionEstado : mesa.DescripcionEstado;
                                    NombreUbicacion = oReader.GetString(5)
                                };
                                listaMesas.Add(row);
                            }
                        }

                        oReader.Dispose();
                    }


                }
                return Ok(new
                {
                    Total = Convert.ToInt32(CantidadTotal),
                    CantPaginas = Convert.ToInt32(Math.Ceiling(CantidadTotal / 10)),
                    Resultado = 200,
                    Mensaje = "Usuarios devueltos correctamente",
                    Lista = listaMesas
                });

            }
            catch (Exception ex)
            {
                //throw;
                return Ok(new
                {
                    resultado = 400,
                    mensaje = "Hubo un problema: " + ex.Message
                });

            }
        }

        // GET: api/Mesas/ObtenerUbicaciones
        [Route("[Action]")]
        [HttpGet]
        public async Task<ActionResult> ObtenerUbicaciones()
        {

            var lstUbicaciones = await _context.Mesas.GroupBy(u => new { u.Ubicacion }).Select(u => new { u.Key.Ubicacion }).ToListAsync();

            try
            {
                return Ok(new
                {
                    resultado = 200,
                    Mensaje = "Se han obtenido las ubicaciones correctamente",
                    lista = lstUbicaciones
                });
            }
            catch (Exception ex)
            {

                return Ok(new
                {
                    resultado = 400,
                    mensaje = "A ocurrido un error, al obtener ubicaciones: " + ex.Message

                });
            }
        }
    }
}
