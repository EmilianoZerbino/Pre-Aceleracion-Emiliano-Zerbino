using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDisney_ChallengeAlkemy.Entidades
{
     /* 
     Punto 1. Modelado de Base de Datos

        ● Película o Serie: deberá tener,
            ○ Imagen.
            ○ Título.
            ○ Fecha de creación.
            ○ Calificación (del 1 al 5).
            ○ Personajes asociados.

    */
    public class Pelicula : IId
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        public String Titulo { get; set; }

        public DateTime Lanzamiento { get; set; }

        public int Calificacion { get; set; }

        [MaxLength(150)]
        public String ImagenUrl { get; set; }

        public ICollection<Personaje> Personajes { get; set; }

        public virtual Genero Genero { get; set; }
    }
}
