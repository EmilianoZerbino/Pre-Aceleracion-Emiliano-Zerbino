using ApiDisney_ChallengeAlkemy.Utilidades;
using ApiDisney_ChallengeAlkemy.ValidationAttributtes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDisney_ChallengeAlkemy.DTOs.PersonajeDTOs
{
    public class PersonajeCreationDTO : IImagen
    {
        public String Nombre { get; set; }
        public int Edad { get; set; }
        public float Peso { get; set; }
        
        public String Historia { get; set; }

        [ExtensionArchivos]
        public IFormFile ImagenUrl { get; set; }
        //public virtual ICollection<Pelicula> Peliculas { get; set; }
    }
}
