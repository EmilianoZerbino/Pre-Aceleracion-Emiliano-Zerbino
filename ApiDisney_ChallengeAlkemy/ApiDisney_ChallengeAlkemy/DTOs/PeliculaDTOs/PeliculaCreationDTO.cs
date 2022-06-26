using ApiDisney_ChallengeAlkemy.Entidades;
using ApiDisney_ChallengeAlkemy.Utilidades;
using ApiDisney_ChallengeAlkemy.ValidationAttributtes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ApiDisney_ChallengeAlkemy.DTOs.PeliculaDTOs
{
    public class PeliculaCreationDTO : IImagen
    {

        public String Titulo { get; set; }

        public DateTime Lanzamiento { get; set; }

        [RangoCalificacion]  //Punto 1  Calificación (del 1 al 5)
        public int Calificacion { get; set; }

        [ExtensionArchivos]
        [PesoArchivo]
        public IFormFile ImagenUrl { get; set; }

        public int[] ProtagonistasIds { get; set; }

        [NotNull]
        public int GeneroId { get; set; }

    }
}
