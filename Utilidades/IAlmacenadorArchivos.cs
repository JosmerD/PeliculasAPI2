using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PeliculasApi.Utilidades
{
    public interface IAlmacenadorArchivos
    {
        Task BorrarArchivo(string contenedor, string ruta);
        Task<string> EditarArchivo(string contenedor, IFormFile archivo, string ruta);
        Task<string> GuardarArchivo(string contenedor, IFormFile archivo);
    }
}