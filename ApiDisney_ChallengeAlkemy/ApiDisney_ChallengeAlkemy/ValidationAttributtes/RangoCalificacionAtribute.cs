using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDisney_ChallengeAlkemy.ValidationAttributtes
{
    public class RangoCalificacionAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int? valor = value as int?;

            if (valor > 5 || valor < 1)
            {
                    return new ValidationResult($"Debe ingresar una calificacion entre 1 y 5");
            }
            return ValidationResult.Success;
        }

    }
}
