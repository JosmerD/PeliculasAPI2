using Microsoft.Extensions.Logging;
using PeliculasApi.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasApi.Repositorio
{
    public class RepositorioEnMemoria:IRepositorio
    {
        private List<Genero> _generos;

        public RepositorioEnMemoria(ILogger<RepositorioEnMemoria> logger)
        {
            _generos = new List<Genero>()
            {
                new Genero(){Id=1,Nombre="Comedia"},
                new Genero(){Id=2,Nombre="Accion"}
            };
            _guid = Guid.NewGuid();
        }
        public Guid _guid;
        public List<Genero> ObtenerTodosLosGeneros()
        {
            return _generos;
        }
        public async Task<Genero> ObtenerGenroPorId(int id)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            return _generos.FirstOrDefault(x => x.Id == id);
        }
        public Guid Obtener()
        {
            return _guid;
        }
    }
}
