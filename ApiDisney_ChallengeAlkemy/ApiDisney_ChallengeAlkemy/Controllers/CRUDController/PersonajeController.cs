using ApiDisney_ChallengeAlkemy.DTOs.PersonajeDTOs;
using ApiDisney_ChallengeAlkemy.Entidades;
using ApiDisney_ChallengeAlkemy.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDisney_ChallengeAlkemy.Controllers.CRUDController
{

        /*
          Punto 4. Creación, Edición y Eliminación de Personajes (CRUD)

          Deberán existir las operaciones básicas de creación, edición y eliminación de personajes.
        */

    [ApiController]
    [Route("/api/[Controller]")]
    [Authorize]
    public class PersonajeController : BaseController<PersonajeCreationDTO, Personaje, PersonajeDTO>
        {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;

        public PersonajeController(ApplicationDbContext applicationDbContext, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
                : base(applicationDbContext, mapper, "Peliculas", almacenadorArchivos)
            {
                this.applicationDbContext = applicationDbContext;
                this.mapper = mapper;
        }

        [HttpGet("characters")] //Nombre Imagen
        public async Task<ActionResult<List<PersonajeDTO>>> GetListadoPersonajes(String name, int? age, int? weight)
        {

            /*
            Punto 3. Listado de Personajes

                El listado deberá mostrar:

                    ● Imagen.
                    ● Nombre.

            El endpoint deberá ser:

                    ● /characters
            */

            if (name == null && age == null && weight == null)
            {
                var listadoPersonajes = await applicationDbContext.Personajes.Select(p =>
                new ViewPersonajeListadoDTO
                {
                    Nombre = p.Nombre,
                    ImagenUrl = p.ImagenUrl
                }).ToListAsync();
                return Ok(listadoPersonajes);
            }
            /*
                    Punto  6. Búsqueda de Personajes

                    Deberá permitir buscar por nombre, y filtrar por edad, peso o películas/series en las que participó.

                    Para especificar el término de búsqueda o filtros se deberán enviar como parámetros de query:

                        ● GET /characters?name=nombre
                        ● GET /characters?age=edad
                        ● GET /characters?movies=idMovie   (Este lo hace por defecto).

                Nota: Se puede filtrar por mas de un atributo a la vez.

                */
            else
            {
                var detallePersonajes = await applicationDbContext.Personajes.Include(p=>p.Peliculas).Select(p =>
             new ViewPersonajeDetalleDTO
             {
                 Id = p.Id,
                 Nombre = p.Nombre,
                 Edad = p.Edad,
                 Peso = p.Peso,
                 Historia = p.Historia,
                 ImagenUrl = p.ImagenUrl,
                 Peliculas = p.Peliculas

             }).ToListAsync();

                if (name != null) {
                    detallePersonajes = detallePersonajes.Where(p => p.Nombre.Contains(name)).ToList();
                }
                if (age != null)
                {
                    detallePersonajes = detallePersonajes.Where(p => p.Edad == age).ToList();
                }
                if (weight != null)
                {
                    detallePersonajes = detallePersonajes.Where(p => p.Peso == weight).ToList();
                }
                
                return Ok(detallePersonajes);
            }
            
        }

        /*
            Punto 5. Detalle de Personaje

            En el detalle deberán listarse todos los atributos del personaje,
            como así también sus películas o series relacionadas.
        */

        [HttpGet("charactersDetail")] 
        public async Task<ActionResult<List<ViewPersonajeDetalleDTO>>> GetDetallePersonajes()
        {
            var detallePersonajes = await applicationDbContext.Personajes.Select(p =>
            new ViewPersonajeDetalleDTO
            {
                Id=p.Id,
                Nombre = p.Nombre,
                Edad = p.Edad,
                Peso = p.Peso,
                Historia = p.Historia,
                ImagenUrl = p.ImagenUrl,
                Peliculas = p.Peliculas

            }).ToListAsync();

            return Ok(detallePersonajes);
        }
    }
    
}
