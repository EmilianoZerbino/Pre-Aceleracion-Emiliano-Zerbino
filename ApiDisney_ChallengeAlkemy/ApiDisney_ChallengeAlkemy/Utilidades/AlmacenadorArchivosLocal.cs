using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDisney_ChallengeAlkemy.Utilidades
{
    public class AlmacenadorArchivosLocal : IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment webHostEnviroment;
        private readonly IHttpContextAccessor httpContextAccesor;

        public AlmacenadorArchivosLocal(IWebHostEnvironment webHostEnviroment, IHttpContextAccessor httpContextAccesor)
        {
            this.webHostEnviroment = webHostEnviroment;
            this.httpContextAccesor = httpContextAccesor;
        }

        public Task Borrar(string ruta, string container)
        {
            String wwwrootPath = webHostEnviroment.WebRootPath;

            if (String.IsNullOrEmpty(wwwrootPath))
            {
                throw new Exception();
            }

            var nombreArchivo = Path.GetFileName(ruta);

            String pathFinal = Path.Combine(wwwrootPath, container, nombreArchivo);

            if (File.Exists(pathFinal))
            {
                File.Delete(pathFinal);
            }

            return Task.CompletedTask;
        }

        public async Task<String> Crear(byte[] file, string contentType, string extension, string container, string nombre)
        {
            String wwwrootPath = webHostEnviroment.WebRootPath;

            if (String.IsNullOrEmpty(wwwrootPath))
            {
                throw new Exception();
            }

            String carpetaArchivo = Path.Combine(wwwrootPath, container);

            if (!Directory.Exists(carpetaArchivo))
            {
                Directory.CreateDirectory(carpetaArchivo);
            }

            String nombreFinal = $"{nombre}{extension}";
            String rutaFinal = Path.Combine(carpetaArchivo, nombreFinal);

            await File.WriteAllBytesAsync(rutaFinal, file);

            String urlActual = $"{httpContextAccesor.HttpContext.Request.Scheme}://{httpContextAccesor.HttpContext.Request.Host}";
            String dbUrl = Path.Combine(urlActual, container, nombreFinal).Replace("\\", "/");

            return dbUrl;

        }
    }
}
