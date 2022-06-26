using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDisney_ChallengeAlkemy.Entidades
{
     /* 
     Punto 1. Modelado de Base de Datos

        ● Género: deberá tener,
            ○ Nombre.
            ○ Imagen.
            ○ Películas o series asociadas.
    */

    public class Genero : IId
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        public String Nombre { get; set; }

        [MaxLength(150)]
        public String ImagenUrl{ get; set; }  //No le encuentro mucho sentido, pero lo deje porque asi lo pedia el Challenge en el punto 1.

        public virtual ICollection<Pelicula> Peliculas { get; set; }
    }
}
