using System;

namespace ApiDisney_ChallengeAlkemy.DTOs.PeliculaDTOs
{
    public class PeliculaDTO
    {

        public int Id { get; set; }

        public String Titulo { get; set; }

        public DateTime Lanzamiento { get; set; }

        public int Calificacion { get; set; }

        public String ImagenUrl { get; set; }

        /*public ICollection<Personaje> Personajes { get; set; }

        public virtual Genero Genero { get; set; }*/
    }
}
