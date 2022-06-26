using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDisney_ChallengeAlkemy.ValidationAttributtes
{
    public class ExtensionArchivosAttribute : ValidationAttribute
    {
        private readonly String[] extensionesValidas;
        private readonly List<String> tiposValidos;

        public ExtensionArchivosAttribute()
        {

            this.extensionesValidas = ConstantesDeApplicacion.TiposDeArchivos.Imagen;
            this.tiposValidos = new List<String>();

            for (int i = 0; i < extensionesValidas.Length; i++)
            {
                this.tiposValidos.Add("image/"+extensionesValidas[i]);
            }
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var formfile = value as IFormFile;

            if (formfile != null)
            {
                if (!tiposValidos.Contains(formfile.ContentType))
                {
                    return new ValidationResult($"Los tipos validos son {String.Join(", ", extensionesValidas)}");
                }
            }
            return ValidationResult.Success;
        }
    }
}
