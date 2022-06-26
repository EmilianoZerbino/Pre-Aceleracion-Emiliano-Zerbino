using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDisney_ChallengeAlkemy
{
    public static class ConstantesDeApplicacion
    {
        public static class ContenedoresDeArchivos
        {
            public const String ContenedorDeImagenes = "Imagenes";

        }

        public static class TiposDeArchivos
        {
            public static readonly String[] Imagen = { "png", "jpg", "jpeg", "gif" };
        }

        public static class PesoDeArchivos
        {
            public static readonly int Imagen = 2048;
        }
    }
}
