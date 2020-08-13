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
    public class SeleccionProductosController : ControllerBase
    {
        private readonly RestaurantBDContext _context;

        public SeleccionProductosController(RestaurantBDContext context)
        {
            _context = context;
        }

        // Obtener listado selección productos: api/SeleccionProductos/ObtenerSeleccionProductos/
        [Route("[Action]")]
        [HttpGet]
        public async Task<ActionResult> ObtenerSeleccionProductos()
        {
            try
            {
                List<object> list = new List<object>();

                var lstSelecciones = await _context.Selecciones.ToListAsync();

                List<SeleccionProducto> seleccionProductos1 = new List<SeleccionProducto>();
                foreach (var item in lstSelecciones)
                {

                    seleccionProductos1 = await _context.SeleccionProductos.Where(s => s.IdSeleccion == item.IdSeleccion).ToListAsync();

                    List<object> Listaproducto1 = new List<object>(); ;
                    foreach (var item2 in seleccionProductos1)
                    {
                        Producto prod = new Producto();
                        prod = await _context.Productos.Where(p => p.IdProducto == item2.IdProducto).FirstOrDefaultAsync();

                        Listaproducto1.Add(new
                        {
                            idProducto = prod.IdProducto,
                            nombre = prod.Nombre,
                            stock = prod.Stock,
                            fechaUltimaReposicion = prod.FechaUltimaReposicion,
                            cantidadRecomendada = prod.CantidadRecomendada
                        });

                    }


                    var x = new
                    {
                        seleccion = new
                        {
                            idSeleccion = item.IdSeleccion,
                            nombre = item.Nombre,
                            valor = item.Valor,
                            tiempo = item.Tiempo,
                            imagen = item.Imagen
                        },
                        productos = Listaproducto1
                    };

                    list.Add(x);

                }

                return Ok(new
                {
                    resultado = 200,
                    mensaje = "Selelecciones disponibles devueltas correctamente",
                    cantidad = lstSelecciones.Count(),
                    lista = list
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    resultado = 400,
                    mensaje = "A ocurrido un problema, exepcion: " + ex.Message
                });
            }

        }

        // Obtener objeto selección productos: api/SeleccionProductos/ObtenerSeleccionProductoById/
        [Route("[Action]/{id}")]
        [HttpGet]
        public async Task<ActionResult> ObtenerSeleccionProductoById(int id)
        {
            try
            {

                var obj = await _context.Selecciones.Where(s => s.IdSeleccion == id).FirstOrDefaultAsync();

                var listProd = await _context.SeleccionProductos.Where(s => s.IdSeleccion == obj.IdSeleccion).Select(s => new {s.IdSeleccionProducto, s.IdSeleccion, s.IdProducto, s.Unidades }).ToListAsync();

                var x = new
                {
                    idSeleccion = obj.IdSeleccion,
                    nombre = obj.Nombre,
                    valor = obj.Valor,
                    tiempo = obj.Tiempo,
                    imagen = obj.Imagen,
                    productos = listProd
                };


                return Ok(new
                {
                    resultado = 200,
                    mensaje = "Selecciones disponibles devueltas correctamente",
                    seleccion = x
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    resultado = 400,
                    mensaje = "A ocurrido un problema, exepcion: " + ex.Message
                });
            }

        }

        // GET: api/SeleccionProductos/ObtenerSeleccionProducto/5
        [Route("[Action]/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<SeleccionProducto>> ObtenerSeleccionProducto(int id)
        {
            var seleccionProducto = await _context.SeleccionProductos.FindAsync(id);

            if (seleccionProducto == null)
            {
                return Ok();
            }

            return seleccionProducto;
        }

        //Editar: api/SeleccionProductos/EditarSeleccionCompleto/{id}
        [Route("[Action]/{id}")]
        [HttpPost("{id}")]
        public async Task<IActionResult> EditarSeleccionCompleto(int id, dynamic post)
        {
            try
            {
                var seleccion = await _context.Selecciones.FindAsync(id);
                seleccion.Imagen = post.imagen;
                seleccion.Nombre = post.nombre;
                seleccion.Tiempo = post.tiempo;
                seleccion.Valor = post.valor;
                _context.Entry(seleccion).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                foreach (var item in post.productos)
                {
                    if (item.idSeleccionProducto != null)
                    {
                        var detalleSeleccion = await _context.SeleccionProductos.FindAsync(id);
                        detalleSeleccion.IdProducto = item.idProducto;
                        detalleSeleccion.Unidades = item.unidades;
                        _context.Entry(detalleSeleccion).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        var detalle = new SeleccionProducto
                        {
                            IdSeleccion = seleccion.IdSeleccion,
                            IdProducto = item.idProducto,
                            Unidades = item.unidades
                        };
                        _context.SeleccionProductos.Add(detalle);
                        await _context.SaveChangesAsync();
                    }
                }
                return Ok(new
                {
                    resultado = 200,
                    mensaje = "Menú editado exitosamente ID: "+seleccion.IdSeleccion
                });
            }
            catch (DbUpdateException ex)
            {
                return Ok(new
                {
                    resultado = 400,
                    mensaje = "A ocurrido un error, Exepcion: " + ex.Message
                });
            }
        }

        // Editar: api/SeleccionProductos/EditarSeleccionProducto/5
        [Route("[Action]/{id}")]
        [HttpPost("{id}")]
        public async Task<IActionResult> EditarSeleccionProducto(int id, SeleccionProducto seleccionProducto)
        {
            if (id != seleccionProducto.IdSeleccion)
            {
                return Ok(new
                {
                    resultado = 400,
                    mensaje = id + " no es igual a seleccion producto ID: " + seleccionProducto.IdSeleccion
                });
            }

            _context.Entry(seleccionProducto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    resultado = 200,
                    mensaje = "Selección Producto editado correctamente"
                });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!SeleccionProductoExists(id))
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

        // Agregar: api/SeleccionProductos/AgregarSeleccionProducto/
        [Route("[Action]")]
        [HttpPost]
        public async Task<ActionResult> AgregarSeleccionProducto(dynamic post)
        {
            try
            {
                var seleccion = new Seleccion { 
                    Imagen = (string)post.imagen,
                    Nombre = (string)post.nombre,
                    Valor = (int)post.valor,
                    Tiempo = (int)post.tiempo
                };
                var productos = post.productos;
                _context.Selecciones.Add(seleccion);
                _context.SaveChanges();
                //Response.WriteAsync("ID: "+seleccion.IdSeleccion);
                foreach (var item in productos)
                {
                    var detalle = new SeleccionProducto
                    {
                        IdSeleccion = seleccion.IdSeleccion,
                        IdProducto = item.idProducto,
                        Unidades = item.cantidad
                    };
                    _context.SeleccionProductos.Add(detalle);
                    await _context.SaveChangesAsync();

                }


                return Ok(new
                {
                    resultado = 200,
                    mensaje = "Se ha almacenado correctamente SeleccionProducto"
                });

            }
            catch (DbUpdateException ex)
            {
                
                    return Ok(new
                    {
                        resultado = 400,
                        Mensaje = "A ocurrido un problema, exepcion: " + ex.Message
                    });
                
            }
            // return CreatedAtAction("GetSeleccionProducto", new { id = seleccionProducto.IdSeleccion }, seleccionProducto);


        }

        // Eliminar: api/SeleccionProductos/EliminarSeleccionProducto/5
        [Route("[Action]/{id}")]
        [HttpPost("{id}")]
        public async Task<ActionResult<SeleccionProducto>> EliminarSeleccionProducto(int id)
        {
            var seleccionProducto = await _context.SeleccionProductos.FindAsync(id);
            if (seleccionProducto == null)
            {
                return Ok(new
                {
                    resultado = 400,
                    mensaje = "No se pudo Eliminar SeleccionProducto"
                });
            }

            _context.SeleccionProductos.Remove(seleccionProducto);
            await _context.SaveChangesAsync();

            //return reserva;
            return Ok(new
            {
                resultado = 200,
                mensaje = "SeleccionProducto eliminada exitosamente"
            });

        }

        private bool SeleccionProductoExists(int id)
        {
            return _context.SeleccionProductos.Any(e => e.IdSeleccion == id);
        }
    }
}
