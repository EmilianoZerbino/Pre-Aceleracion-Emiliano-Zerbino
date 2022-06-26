using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDisney_ChallengeAlkemy.Utilidades
{
    interface IImagen
    {
        public IFormFile ImagenUrl { get; set; }
    }
}
