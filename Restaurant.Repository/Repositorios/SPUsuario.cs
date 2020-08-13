using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Repository.Repositorios
{
    public class SPUsuario
    {
        private readonly RestaurantBDContext _context;

        public SPUsuario(RestaurantBDContext context)
        {
            _context = context;
        }

        //public async Task<DataSet> ObtencionUsuarioDetalle(int rut)
        //{
        //    DataSet retornado = new DataSet();

        //    SqlConnection conn = (SqlConnection)_context.Database.GetDbConnection();
        //    SqlCommand cmd = conn.CreateCommand();
        //    conn.Open();
        //    cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //    cmd.CommandText = "ObtencionUsuario";

        //    cmd.ExecuteNonQuery();

        //    SqlDataAdapter query = new SqlDataAdapter(cmd);

        //    query.Fill(retornado);
        //    conn.Close();

        //    return retornado;
        //}


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
