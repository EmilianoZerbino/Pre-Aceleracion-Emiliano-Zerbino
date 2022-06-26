using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDisney_ChallengeAlkemy.Entidades
{
     /* 
     Punto 1. Modelado de Base de Datos

        ● Personaje: deberá tener,
            ○ Imagen.
            ○ Nombre.
            ○ Edad.
            ○ Peso.
            ○ Historia.
            ○ Películas o series asociadas.
    */
    public class Personaje : IId
    {

        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        public String Nombre { get; set; }
        public int Edad { get; set; }
        public float Peso { get; set; }
        [MaxLength(150)]
        public String Historia { get; set; }
        [MaxLength(150)]
        public String ImagenUrl { get; set; }
        public virtual ICollection<Pelicula> Peliculas { get; set; }
    }
}
