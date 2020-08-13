using Restaurant.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Repository.Repositorios
{
    public class PerfilDTO
    {
        public int IdPerfil { get; set; }
        public string NombrePerfil { get; set; }
        public List<UsuarioDTO> Usuarios { get; set; }

        public PerfilDTO()
        {
                
        }

    }
}
