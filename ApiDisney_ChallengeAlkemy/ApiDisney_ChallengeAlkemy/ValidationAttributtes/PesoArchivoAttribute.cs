using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDisney_ChallengeAlkemy.ValidationAttributtes
{
    public class PesoArchivoAttribute : ValidationAttribute
    {
        private readonly double pesoArchivo;

        public PesoArchivoAttribute()
        {
            this.pesoArchivo = ConstantesDeApplicacion.PesoDeArchivos.Imagen;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var formfile = value as IFormFile;

            if (formfile != null)
            {
                if (formfile.Length / 1024 > pesoArchivo)
                {
                    return new ValidationResult($"El Peso máximo Admitido es de: {pesoArchivo} Kb / {pesoArchivo/1024} Mb");
                }
            }
            return ValidationResult.Success;
        }
    }
}
