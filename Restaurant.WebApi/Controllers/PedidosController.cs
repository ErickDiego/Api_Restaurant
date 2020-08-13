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
    public class PedidosController : ControllerBase
    {
        private readonly RestaurantBDContext _context;

        public PedidosController(RestaurantBDContext context)

        {
            _context = context;
        }

        // GET: api/Pedidos/ObtenerPedidos
        [Route("[Action]")]
        [HttpGet]
        public async Task<ActionResult> ObtenerPedidos()
        {
            try
            {
                int estadoPedido = 1;

                List<Pedido> ListaPedidos = new List<Pedido>();
                List<Detalle> ListaDetalles = new List<Detalle>();

                List<object> salida = new List<object>();

                //ListaPedidos = await _context.Pedidos.Where(p => p.IdEstadoPedido == estadoPedido).ToListAsync();
                ListaPedidos = await _context.Pedidos.Where(p => p.IdEstadoPedido != 3).ToListAsync();

                foreach (var item in ListaPedidos)
                {
                    object obj = new object();
                    List<Detalle> detalles = new List<Detalle>();

                    detalles = await _context.Detalles.Where(d => d.IdPedido == item.IdPedido).ToListAsync();

                    List<Object> Listaseleccion = new List<Object>();
                    foreach (var item2 in detalles)
                    {
                        Seleccion x = await _context.Selecciones.Where(s => s.IdSeleccion == item2.IdSeleccion).FirstAsync();

                        var Y = new
                        {
                            item2.Cantidad,
                            x.IdSeleccion,
                            x.Nombre,
                            x.Valor,
                            x.Tiempo,
                            x.Imagen
                        };
                        Listaseleccion.Add(Y);
                    }

                    Estado estado = await _context.Estados.Where(e => e.IdEstado == item.IdEstadoPedido).FirstAsync();

                    obj = new
                    {
                        item.IdMesa,
                        item.IdPedido,
                        item.IdEstadoPedido,
                        nombreEstado = estado.Nombre,
                        Listaseleccion
                    };
                    salida.Add(obj);
                }

                return Ok(new
                {
                    resultado = 200,
                    mensaje = "Se ha obtenido los pedidos pendientes correctamente",
                    cantidadDePedidos = ListaPedidos.Count(),
                    detalle = salida
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

        // GET: api/Pedidoes/5
        [Route("[Action]/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult> ObtenerPedido(int id)
        {
            try
            {

                Pedido Pedidos = new Pedido();
                List<Detalle> ListaDetalles = new List<Detalle>();

                List<object> salida = new List<object>();

                Pedidos = await _context.Pedidos.Where(p => p.IdPedido == id).FirstAsync();


                object obj = new object();
                List<Detalle> detalles = new List<Detalle>();


                detalles = await _context.Detalles.Where(d => d.IdPedido == Pedidos.IdPedido).ToListAsync();
                int total = 0;

                List<Object> Listaseleccion = new List<Object>();

                foreach (var item2 in detalles)
                {
                    Seleccion x = await _context.Selecciones.Where(s => s.IdSeleccion == item2.IdSeleccion).FirstAsync();

                    var Y = new
                    {
                        item2.Cantidad,
                        x.IdSeleccion,
                        x.Nombre,
                        x.Valor,
                        x.Tiempo,
                        x.Imagen
                    };

                    total += (x.Valor * item2.Cantidad);
                    Listaseleccion.Add(Y);

                }

                obj = new
                {
                    Pedidos.IdMesa,
                    Pedidos.IdPedido,
                    Listaseleccion
                };
                salida.Add(obj);


                return Ok(new
                {
                    resultado = 200,
                    mensaje = "Se ha obtenido el Pedido correctamente",
                    MontoTotal = total,
                    detalle = salida
                });
            }
            catch (Exception ex)
            {

                if (!PedidoExists(id))
                {
                    return Ok(new
                    {
                        resultado = 404, 
                        mensaje = "No se encuentra pedido con el ID: " + id
                    });
                }
                else
                {
                    return Ok(new
                    {
                        resultado = 400,
                        mensaje = "A ocurrido un problema en la obtencion del pedido, exepcion: " + ex.Message
                    });
                }
            }

        }


        // PUT: api/Pedidos/5
        [Route("[Action]/{id}")]
        [HttpPost("{id}")]
        public async Task<ActionResult> EditarPedido(int id, [FromBody] Pedido pedido)
        {
            if (id != pedido.IdPedido)
            {
                return BadRequest(new
                {
                    resultado = 400,
                    mensaje = "Pedido no existe"
                }); ;
            }

            _context.Entry(pedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    resultado = 200,
                    Mensaje = "Pedido n° " + pedido.IdPedido + " editado con exito",
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(id))
                {
                    return BadRequest(new
                    {
                        resultado = 400,
                        mensaje = "Error al actualizar pedido"
                    }); ;
                }
                else
                {
                    return BadRequest(new
                    {
                        resultado = 400,
                        mensaje = "Error inesperado al editar pedido"
                    });
                }
            }
        }

        // POST: api/Pedidoes
        [Route("[Action]")]
        [HttpPost]
        public async Task<ActionResult> AgregarPedido(Pedido pedido)
        {
            try
            {
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();


                return Ok(new
                {
                    resultado = 200,
                    Mensaje = "Se a almacenado correctamente el pedido"
                });

                //return CreatedAtAction("GetPedido", new { id = pedido.IdPedido }, pedido);
            }
            catch (Exception ex)
            {

                return Ok(new
                {
                    resultado = 400,
                    mensaje = "Error: " + ex.Message
                });
            }
        }

        // DELETE: api/Pedidoes/5
        [HttpPost("{id}")]
        [Route("[Action]/{id}")]
        public async Task<ActionResult<Pedido>> EliminarPedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return Ok();
            }

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return pedido;
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.IdPedido == id);
        }

        [Route("[Action]")]
        [HttpGet]
        public async Task<ActionResult> ObtenerUltimoId()
        {
            try
            {
                var IdMaximo = _context.Pedidos.Max(p => p.IdPedido);

                if (IdMaximo > 0)
                {
                    return Ok(new
                    {
                        Resultado = 200,
                        Mensaje = "Se ha obtenido correctamente el ultimo ID de los pedidos",
                        Id = IdMaximo
                    });
                }
                else
                {
                    return Ok(new
                    {
                        Resultado = 200,
                        Mensaje = "Se ha obtenido correctamente el ultimo ID de los pedidos",
                        Id = 0
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Resultado = 400,
                    Mensaje = "A ocurrido un problema, Excepcion: " + ex.Message
                });
            }
        }


    }
}
