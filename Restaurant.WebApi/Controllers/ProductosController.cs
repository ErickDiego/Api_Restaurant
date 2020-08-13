using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Models;
using System.Globalization;

namespace Restaurant.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly RestaurantBDContext _context;

        public ProductosController(RestaurantBDContext context)
        {
            _context = context;
        }

        // Obtener todos: api/Productos
        [Route("[Action]")]
        [HttpGet]
        public async Task<ActionResult> ObtenerProductos()
        {
            try
            {
                return Ok(new
                {
                    resultado = 200,
                    Total = await _context.Productos.CountAsync(),
                    Mensaje = "Se ha obtenido los productos correctamente",
                    lista = await _context.Productos.ToListAsync()
                });
                //return await _context.Productos.ToListAsync();
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

        // Obtener por Id: api/Productos/5
        [Route("[Action]/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult> ObtenerProducto(int id)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(id);

                return Ok(new
                {
                    resultado = 200,
                    mensaje = "Producto obtenido correctamente ",
                    producto = new
                    {
                        producto.IdProducto,
                        producto.Nombre,
                        producto.Stock,
                        FechaUltimaReposicion = producto.FechaUltimaReposicion.ToString("dd/MM/yyy"),
                        producto.CantidadRecomendada
                    }
                }) ;

            }
            catch (Exception ex)
            {

                if (!ProductoExists(id))
                {
                    return Ok(new
                    {
                        resultado = 404,
                        mensaje = "No se a podido encontrar el Producto con ID: " + id
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

        // Actualizar: api/Productos/5
        [Route("[Action]/{id}")]
        [HttpPost("{id}")]
        public async Task<IActionResult> EditarProducto(int id, Producto producto)
        {
            if (id != producto.IdProducto)
            {
                return Ok();
            }


            try
            {

                Producto producto1 = await _context.Productos.FirstOrDefaultAsync(p => p.IdProducto == producto.IdProducto);

                producto1.IdProducto = producto1.IdProducto;
                producto1.Nombre = producto.Nombre == "" ? producto1.Nombre: producto.Nombre; //condition ? consequent : alternative
                producto1.Stock = producto.Stock == 0 ? producto1.Stock : producto.Stock; //condition ? consequent : alternative
                producto1.FechaUltimaReposicion = DateTime.Now;
                producto1.CantidadRecomendada = producto.CantidadRecomendada == 0 ? producto1.CantidadRecomendada : producto.CantidadRecomendada; //condition ? consequent : alternative

                _context.Entry(producto1).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    resultado = 200,
                    mensaje = "Producto Editado exitosamente"
                }
                    );
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ProductoExists(id))
                {
                    return Ok();
                }
                else
                {
                    return Ok(new
                    {
                        resultado = 400,
                        mensaje = "A ocurrido un error, exepcion: " + ex.Message
                    });
                }
            }

        }

        // Crear: api/Productos
        [Route("[Action]")]
        [HttpPost]
        public async Task<ActionResult> AgregarProducto(Producto producto)
        {
            try
            {
                producto.FechaUltimaReposicion = DateTime.Now;

                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();

                //return CreatedAtAction("GetProducto", new { id = producto.IdProducto }, producto
                return Ok(new
                {
                    resultado = 200,
                    mensaje = "Producto guardado exitosamente"
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    resultado = 400,
                    mensaje = "Problemas al ingresa el producto, exeption: " + ex.Message
                });
            }
        }

        // Eliminar: api/Productos/5
        [Route("[Action]/{id}")]
        [HttpPost("{id}")]
        public async Task<ActionResult> EliminarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return Ok();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                resultado = 200,
                mensaje = "Producto eliminado exitosamente"
            });
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.IdProducto == id);
        }


        ///<summary>
        ///Paginacion de el listado de productos
        ///</summary>
        ///<return>
        ///Devuelve lotes de datos segun la pagina
        ///</return>
        ///<param name="pagina">
        //// "api/productos/listadoDeProductos/{pagina}"
        ///</param>
        [Route("[Action]/{pagina}")]
        [HttpGet("{pagina}")]
        public async Task<ActionResult> ListadoDeProductos(int pagina)
        {
            List<object> listaProductos = new List<object>();
            double CantidadTotal = await _context.Productos.CountAsync();
            try
            {
                using (SqlConnection conn = (SqlConnection)_context.Database.GetDbConnection())
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "ListadoProductos";
                        cmd.Parameters.AddWithValue("@PageNumber", pagina);

                        DbDataReader oReader = await cmd.ExecuteReaderAsync();

                        if (oReader.HasRows)
                        {
                            while (await oReader.ReadAsync())
                            {
                                var row = new 
                                {
                                    IdProducto = oReader.GetInt32(0),
                                    Nombre = oReader.GetString(1),
                                    Stock = oReader.GetInt32(2),
                                    FechaUltimaReposicion = oReader.GetDateTime(3).ToString("dd/MM/yyy hh:mm"), 
                                    CantidadRecomendada = oReader.GetInt32(4),
                                };
                                listaProductos.Add(row);
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
                    Mensaje = "Productos devueltos correctamente",
                    response = listaProductos
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
