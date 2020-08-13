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
using Restaurant.Models.Procedure;
using Restaurant.Repository.Repositorios;

namespace Restaurant.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly RestaurantBDContext _context;

        public UsuariosController(RestaurantBDContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [Route("[Action]")]
        [HttpGet]
        public async Task<ActionResult> ObtenerUsuarios()
        {
            //return await _context.Usuarios.ToListAsync();
            try
            {
                List<Usuario> ListadoDeUsuarios = new List<Usuario>();
                ListadoDeUsuarios = await _context.Usuarios.ToListAsync();

                List<object> listaUsuarios = new List<object>();
                foreach (var item in ListadoDeUsuarios)
                {
                    var row = new
                    {
                        Rut = item.Rut,
                        IdPerfil = item.IdPerfil,
                        Nombre = item.Nombre,
                        ApellidoMaterno = item.ApellidoMaterno,
                        ApellidoPaterno = item.ApellidoPaterno,
                        Activo = item.Activo,
                        //Contrasena = oReader.GetString(6),
                        NombreUsuario = item.NombreUsuario,
                        Correo = item.Correo,
                        Telefono = item.Telefono
                    };
                    listaUsuarios.Add(row);
                }

                return Ok(new
                {
                    resultado = 200,
                    Total = await _context.Usuarios.CountAsync(),
                    Mensaje = "Se ha obtenido los Usuarios correctamente",
                    lista = listaUsuarios
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

        // GET: api/Usuarios/5   
        [Route("[Action]/{rut}")]
        [HttpGet("{rut}")]
        public async Task<ActionResult> ObtenerUsuario(int rut)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(rut);

                return Ok(new
                {
                    resultado = 200,
                    mensaje = "Usuario obtenido correctamente ",
                    usuario = new
                    {
                        usuario.Rut,
                        usuario.IdPerfil,
                        usuario.Nombre,
                        usuario.ApellidoPaterno,
                        usuario.ApellidoMaterno,
                        usuario.Activo,
                        usuario.NombreUsuario,
                        usuario.Correo,
                        usuario.Telefono
                    }
                });

            }
            catch (Exception ex)
            {

                if (!UsuarioExists(rut))
                {
                    return Ok(new
                    {
                        resultado = 404,
                        mensaje = "No se a podido encontrar el usuario con Rut: " + rut
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

        // PUT: api/Usuarios/5
        [Route("[Action]/{id}")]
        [HttpPost("{id}")]
        public async Task<IActionResult> EditarUsuario(int id, [FromBody]Usuario usuario)
        {
            if (id != usuario.Rut)
            {
                return Ok(new
                {
                    resultado = 400,
                    mensaje = id + " no es igual a el rut: " + usuario.Rut
                });
            }


            try
            {

                Usuario usuario1 = await _context.Usuarios.FirstOrDefaultAsync(u => u.Rut == usuario.Rut);

                usuario1.Rut = usuario1.Rut;
                usuario1.IdPerfil = usuario.IdPerfil == 0 ? usuario1.IdPerfil : usuario.IdPerfil; //condition ? consequent : alternative
                usuario1.Nombre = usuario.Nombre == "" ? usuario1.Nombre : usuario.Nombre; //condition ? consequent : alternative
                usuario1.ApellidoPaterno = usuario.ApellidoPaterno == "" ? usuario1.ApellidoPaterno : usuario.ApellidoPaterno; //condition ? consequent : alternative
                usuario1.ApellidoMaterno = usuario.ApellidoMaterno == "" ? usuario1.ApellidoMaterno : usuario.ApellidoMaterno; //condition ? consequent : alternative
                usuario1.Activo = usuario.Activo == 0 ? usuario1.Activo : usuario.Activo; //condition ? consequent : alternative
                usuario1.Contrasena = usuario.Contrasena == "" ? usuario1.Contrasena : usuario.Contrasena; //condition ? consequent : alternative
                usuario1.NombreUsuario = usuario.NombreUsuario == "" ? usuario1.NombreUsuario : usuario.NombreUsuario; //condition ? consequent : alternative
                usuario1.Correo = usuario.Correo == "" ? usuario1.Correo : usuario.Correo; //condition ? consequent : alternative
                usuario1.Telefono = usuario.Telefono == "" ? usuario1.Telefono : usuario.Telefono; //condition ? consequent : alternative


                _context.Entry(usuario1).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    resultado = 200,
                    Mensaje = "Usuario Editado Correctamente"
                });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!UsuarioExists(id))
                {
                    return Ok(new
                    {
                        resultado = 404,
                        mensaje = "No se a encontrado el usuario con id: " + id.ToString()
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

        // POST: api/Usuarios
        [Route("[Action]")]
        [HttpPost]
        public async Task<ActionResult> AgregarUsuario([FromBody]UsuarioJsonDTO usuarioJsonDTO)
        {

            var Rut =  Int32.Parse(usuarioJsonDTO.rut.Replace("-", "").ToUpper());
            Usuario usuario = new Usuario
            {
                Rut = Rut,
                IdPerfil = usuarioJsonDTO.idPerfil,
                Nombre = usuarioJsonDTO.nombre,
                ApellidoPaterno = usuarioJsonDTO.apellidoPaterno,
                ApellidoMaterno = usuarioJsonDTO.apellidoMaterno,
                Activo = usuarioJsonDTO.activo,
                NombreUsuario = usuarioJsonDTO.nombreUsuario,
                Correo = usuarioJsonDTO.correo,
                Telefono = usuarioJsonDTO.telefono,
                Contrasena = usuarioJsonDTO.contrasena
            };


            _context.Usuarios.Add(usuario);
            try
            {
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    resultado = 200,
                    mensaje = "Usuarios guardado exitosamente"
                });

            }
            catch (DbUpdateException ex)
            {
                if (UsuarioExists(usuario.Rut))
                {
                    //return Conflict();

                    return Ok(new
                    {
                        resultado = 400,
                        mensaje = "Usuarios ya se encuentra Registrado"
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

        // DELETE: api/Usuarios/5
        [HttpPost("{id}")]
        [Route("[Action]/{id}")]
        public async Task<ActionResult> EliminarUsuario(int id)
        {

            try
            {
                Usuario usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Rut == id);

                usuario.Rut = usuario.Rut;
                usuario.IdPerfil = usuario.IdPerfil;
                usuario.Nombre = usuario.Nombre;
                usuario.ApellidoPaterno = usuario.ApellidoPaterno;
                usuario.ApellidoMaterno = usuario.ApellidoMaterno;
                usuario.Activo = 0; //          0 Inactivo -------- 1 Activo
                usuario.Contrasena = usuario.Contrasena;
                usuario.NombreUsuario = usuario.NombreUsuario;
                usuario.Correo = usuario.Correo;
                usuario.Telefono = usuario.Telefono;


                _context.Entry(usuario).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    resultado = 200,
                    mensaje = "Usuarios eliminado exitosamente"
                });

            }
            catch (Exception ex)
            {
                if (!UsuarioExists(id))
                {
                    return Ok(new
                    {
                        resultado = 404,
                        mensaje = "No existe el Usuario con Rut: " + id
                    });
                }
                else
                {
                    return Ok(new
                    {
                        resultado = 400,
                        mensaje = "No se pudo Eliminar el Usuario, exepcion: " + ex.Message
                    });
                }
            }
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Rut == id);
        }


        [Route("[Action]/{pagina}")]
        [HttpGet("{pagina}")]
        public async Task<ActionResult> ListadoDeUsuarios(int pagina)
        {
            List<object> listaUsuarios = new List<object>();

            double CantidadTotal = await _context.Usuarios.Where(u => u.Activo == 1).CountAsync();
            try
            {
                using (SqlConnection conn = (SqlConnection)_context.Database.GetDbConnection())
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "ListadoUsuarios";
                        cmd.Parameters.AddWithValue("@PageNumber", pagina);

                        DbDataReader oReader = await cmd.ExecuteReaderAsync();

                        if (oReader.HasRows)
                        {
                            while (await oReader.ReadAsync())
                            {
                                var row = new
                                {
                                    Rut = oReader.GetInt32(0),
                                    IdPerfil = oReader.GetInt32(1),
                                    Nombre = oReader.GetString(2),
                                    ApellidoMaterno = oReader.GetString(3),
                                    ApellidoPaterno = oReader.GetString(4),
                                    Activo = oReader.GetInt32(5),
                                    //Contrasena = oReader.GetString(6),
                                    NombreUsuario = oReader.GetString(7),
                                    Correo = oReader.GetString(8),
                                    Telefono = oReader.GetString(9)
                                };
                                listaUsuarios.Add(row);
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
                    Mensaje = "Usuarios devueltos correctamente",
                    Lista = listaUsuarios
                });
            }
            catch (Exception ex)
            {
                // throw;
                return Ok(new
                {
                    resultado = 400,
                    mensaje = "Hubo un problema: " + ex.Message
                });

            }
        }


        [Route("[Action]")]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] Login login)
        {
            try
            {
                Usuario usuario = await _context.Usuarios.Where(u => u.NombreUsuario == login.nombreUsuario
                                                                & u.Contrasena == login.contrasena).FirstAsync();
                Perfil perfil = await _context.Perfiles.Where(p => p.IdPerfil == usuario.IdPerfil).FirstAsync();

                List<PerfilXModulo> pxm = await _context.PerfilXModulos.Where(p => p.IdPerfil == perfil.IdPerfil).ToListAsync();

                List<Modulo> ListaModulos = new List<Modulo>();

                foreach (var item in pxm)
                {
                    Modulo modulo = new Modulo();

                    modulo = await _context.Modulos.Where(m => m.IdModulo == item.IdModulo).FirstAsync();

                    ListaModulos.Add(modulo);
                }

                return Ok(new
                {
                    resultado = 200,
                    mensaje = "Se a logueado correctamente",
                    Usuario = new
                    {
                        usuario.Rut,
                        usuario.IdPerfil,
                        usuario.Nombre,
                        usuario.ApellidoPaterno,
                        usuario.ApellidoMaterno,
                        usuario.Activo,
                        // usuario.Contrasena,
                        usuario.NombreUsuario,
                        usuario.Correo,
                        usuario.Telefono
                    },
                    Perfil = new
                    {
                        perfil.IdPerfil,
                        nombrePerfil = perfil.NombrePefil
                    },
                    Modulos = ListaModulos

                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    resultado = 400,
                    mensaje = "Credenciales invalidas, intentar nuevamente"

                });
            }
        }

    }
}
