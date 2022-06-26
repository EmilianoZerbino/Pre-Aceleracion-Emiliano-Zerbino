using ApiDisney_ChallengeAlkemy.DTOs.GeneroDTOs;
using ApiDisney_ChallengeAlkemy.Entidades;
using ApiDisney_ChallengeAlkemy.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiDisney_ChallengeAlkemy.Controllers.CRUDController
{
    [ApiController]
    [Route("/api/[Controller]")]
    [Authorize]
    public class GeneroController : BaseController<GeneroCreationDTO, Genero, GeneroDTO>
    {
        public GeneroController(ApplicationDbContext applicationDbContext, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
            : base(applicationDbContext, mapper, "Generos", almacenadorArchivos)
        {

        }

    }
}
