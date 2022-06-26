using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDisney_ChallengeAlkemy.DTOs.PeliculaDTOs
{
    public class ViewPeliculaListadoDTO
    {
        public String Titulo { set; get; }
        public DateTime Lanzamiento { get; set; }
        public String ImagenUrl { get; set; }
    }
}
