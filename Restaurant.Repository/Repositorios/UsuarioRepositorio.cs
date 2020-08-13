using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Repository.Repositorios
{
    public class UsuarioRepositorio
    {

        private readonly RestaurantBDContext _context;

        public UsuarioRepositorio(RestaurantBDContext context)
        {
            _context = context;
        }

        private readonly SPUsuario _sPUsuario;

        // GET: api/Usuarios
        public async Task<IEnumerable<Usuario>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuarios/5
        //[HttpGet("{id}")]
        public async Task<Usuario> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

         
            return usuario;
        }

        // PUT: api/Usuarios/5
        public async Task<bool> PutUsuario(int id, Usuario usuario)
        {
            var producto = await _context.Usuarios.SingleOrDefaultAsync(c => c.Rut == id);

            try
            {
                return (await _context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception excepcion)
            {
                ;
            }
            return false;
        }

        // POST: api/Usuarios
        public async Task<Usuario> PostUsuario(Usuario usuario)
        {
             _context.Usuarios.Add(usuario);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception excepcion)
            {
                ;
            }

            return usuario;

        }

        // DELETE: api/Usuarios/5
        public async Task<Usuario> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
       

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Rut == id);
        }

        public async Task<DataSet> ObtencionUsuarioDetalle(int rut)
        {
            DataSet UserCompleto = new DataSet();

            using (SqlConnection conn = (SqlConnection)_context.Database.GetDbConnection())
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ObtencionUsuario";
                    cmd.Parameters.AddWithValue("@rutUsuario", rut);

                    DbDataReader oReader = await cmd.ExecuteReaderAsync();

                    if (oReader.HasRows)
                    {

                    }

                    oReader.Dispose();
                }
            }

            return UserCompleto;
        }

    }
}
