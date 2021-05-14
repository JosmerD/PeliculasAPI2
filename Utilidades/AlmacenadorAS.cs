using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace PeliculasApi.Utilidades
{
    public class AlmacenadorAS : IAlmacenadorArchivos
    {
        private string conectionString;
        public AlmacenadorAS(IConfiguration configuration)
        {
            conectionString = configuration.GetConnectionString("AzureStorage");
        }
        
        public async Task<string> GuardarArchivo(string contenedor, IFormFile archivo)
        {
            var cliente = new BlobContainerClient(conectionString, contenedor);
            await cliente.CreateIfNotExistsAsync();
            cliente.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            var extension = Path.GetExtension(archivo.FileName);
            var archivoNombre = $"{Guid.NewGuid()}{extension}";
            var blob = cliente.GetBlobClient(archivoNombre);
            await blob.UploadAsync(archivo.OpenReadStream());
            return blob.Uri.ToString();
        }
        public async Task BorrarArchivo(string contenedor, string ruta)
        {
            if (string.IsNullOrEmpty(ruta))
            {
                return;
            }
            var cliente = new BlobContainerClient(conectionString, contenedor);
            await cliente.CreateIfNotExistsAsync();
            var archivo = Path.GetFileName(ruta);
            var blob = cliente.GetBlobClient(archivo);
            await blob.DeleteIfExistsAsync();
        }
        public async Task<string> EditarArchivo(string contenedor, IFormFile archivo, string ruta)
        {
            await BorrarArchivo(ruta, contenedor);
            return await GuardarArchivo(contenedor, archivo);
        }
    }
}
