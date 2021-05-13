using PeliculasApi.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasApi.Repositorio
{
    public interface IRepositorio
    {
        Guid Obtener();
        Task<Genero> ObtenerGenroPorId(int id);
        List<Genero> ObtenerTodosLosGeneros();
    }
}
