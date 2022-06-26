using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDisney_ChallengeAlkemy.DTOs.PersonajeDTOs
{
    public class PersonajeDTO
    {
        public int Id { get; set; }
        public String Nombre { get; set; }
        public int Edad { get; set; }
        public float Peso { get; set; }
        public String Historia { get; set; }
        public String ImagenUrl { get; set; }
        //public virtual ICollection<Pelicula> Peliculas { get; set; }
    }
}
