using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDisney_ChallengeAlkemy.ValidationAttributtes
{
    public class NotNullAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int? valor = value as int?;

            if (valor==null)
            {
                return new ValidationResult($"Debe ingresar un Atributo en este Campo");
            }
            return ValidationResult.Success;
        }

    }
}

