using ApiDisney_ChallengeAlkemy.Entidades;
using ApiDisney_ChallengeAlkemy.Utilidades;
using ApiDisney_ChallengeAlkemy.ValidationAttributtes;
using Microsoft.AspNetCore.Http;
using System;

namespace ApiDisney_ChallengeAlkemy.DTOs.GeneroDTOs
{
    public class GeneroCreationDTO : IImagen
    {

        public String Nombre { get; set; }

        [ExtensionArchivos]
        [PesoArchivo]
        public IFormFile ImagenUrl { get; set; }

        //public virtual ICollection<Pelicula> Peliculas { get; set; }
    }
}
