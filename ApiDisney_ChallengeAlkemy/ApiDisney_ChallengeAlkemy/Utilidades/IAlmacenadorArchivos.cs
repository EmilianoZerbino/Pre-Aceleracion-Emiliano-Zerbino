using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDisney_ChallengeAlkemy.Utilidades
{
    public interface IAlmacenadorArchivos
    {
        public Task<String> Crear(byte[] file, String contentType, String extension, String container, String nombre);
        public Task Borrar(String ruta, String container);
    }
}
