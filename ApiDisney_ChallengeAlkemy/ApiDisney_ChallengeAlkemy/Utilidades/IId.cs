using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDisney_ChallengeAlkemy.Entidades
{
    public interface IId
    {
        public int Id { get; set; }

        public String ImagenUrl { get; set; }
    }
}
