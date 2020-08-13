using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Repository.Repositorios;

namespace Restaurant.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilsController : ControllerBase
    {
        private readonly RestaurantBDContext _context;

        public PerfilsController(RestaurantBDContext context)
        {
            _context = context;
        }

        // GET: api/perfils/ObtenerPerfiles
        [Route("[Action]")]
        [HttpGet]
        public async Task<ActionResult> ObtenerPerfiles()
        {
            try
            {
                var ePerfil = await _context.Perfiles.ToListAsync();
                var eUsuario = await _context.Usuarios.ToListAsync();
                List<PerfilDTO> lstPerfiles = new List<PerfilDTO>();


                foreach (var perfil in ePerfil)
                {
                    List<UsuarioDTO> lstUsuarios = new List<UsuarioDTO>();
                    PerfilDTO perfilDTO = new PerfilDTO();

                    perfilDTO.IdPerfil = perfil.IdPerfil;
                    perfilDTO.NombrePerfil = perfil.NombrePefil;

                    List<Usuario> eUsuarios = eUsuario.Where(us => us.IdPerfil == perfil.IdPerfil).ToList();

                    foreach (var usuario in eUsuarios)
                    {

                        UsuarioDTO usuarioDTO = new UsuarioDTO
                        {
                            Rut = usuario.Rut,
                            IdPerfil = usuario.IdPerfil,
                            Nombre = usuario.Nombre,
                            ApellidoPaterno = usuario.ApellidoPaterno,
                            ApellidoMaterno = usuario.ApellidoMaterno,
                            Activo = usuario.Activo,
                            NombreUsuario = usuario.NombreUsuario,
                            Correo = usuario.Correo,
                            Telefono = usuario.Telefono
                        };
                        lstUsuarios.Add(usuarioDTO);
                        
                    }

                    perfilDTO.Usuarios = lstUsuarios;

                    lstPerfiles.Add(perfilDTO);
                }
                

                return Ok(new
                {
                    resultado = 200,
                    Mensaje = "Se han obtenido los perfiles correctamente",
                    lista = lstPerfiles
                });

            }
            catch (Exception ex)
            {

                return Ok(new
                {
                    Resultado = 400,
                    Mensaje = "Se ha producido un problema al obtener los perfiles, expecion: " + ex.Message
                });
            }
        }

        // GET: api/Perfils/5
        [Route("[Action]/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetPerfil(int id)
        {
            var perfil = await _context.Perfiles.FindAsync(id);
            

            if (perfil == null)
            {
                return Ok();
            }

            return Ok(new {
                resultado = 200,
                mensaje = "Seleccion ha obtenido el perfil correctamente",
                Perfil = perfil
            });
        }

        // POST: api/Perfils/EditarPerfil/5
        [Route("[Action]/{id}")]
        [HttpPost("{id}")]
        public async Task<IActionResult> EditarPerfil(int id, Perfil perfil)
        {
            if (id != perfil.IdPerfil)
            {
                return Ok();
            }

            _context.Entry(perfil).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PerfilExists(id))
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

        // POST: api/Perfils/AgregarPerfil
        [Route("[Action]")]
        [HttpPost]
        public async Task<ActionResult> AgregarPerfil(Perfil perfil)
        {
            _context.Perfiles.Add(perfil);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerfil", new { id = perfil.IdPerfil }, perfil);
        }

        // POST: api/Perfils/EliminarPerfil/5
        [Route("[Action]/{id}")]
        [HttpPost("{id}")]
        public async Task<ActionResult<Perfil>> EliminarPerfil(int id)
        {
            var perfil = await _context.Perfiles.FindAsync(id);
            if (perfil == null)
            {
                return Ok();
            }

            _context.Perfiles.Remove(perfil);
            await _context.SaveChangesAsync();

            return perfil;
        }

        private bool PerfilExists(int id)
        {
            return _context.Perfiles.Any(e => e.IdPerfil == id);
        }
    }
}
