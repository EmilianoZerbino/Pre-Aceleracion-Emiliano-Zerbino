using ApiDisney_ChallengeAlkemy.DTOs.PeliculaDTOs;
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
     Punto 9. Creación, Edición y Eliminación de Película / Serie (CRUD)

     Deberán existir las operaciones básicas de creación, edición y eliminación de películas o series.
    */

    [ApiController]
    [Route("/api/[Controller]")]
    [Authorize]
    public class PeliculaController : BaseController<PeliculaCreationDTO, Pelicula, PeliculaDTO>
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;

        public PeliculaController(ApplicationDbContext applicationDbContext, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
            : base(applicationDbContext, mapper, "Peliculas", almacenadorArchivos)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        /*
         Punto 7. Listado de Películas
         
         Deberá mostrar solamente los campos imagen, título y fecha de creación.

         El endpoint deberá ser:

            ● GET /movies
        */

    [HttpGet("movies")] //Titulo, FechaCreacion, Imagen
        public async Task<ActionResult<List<PeliculaDTO>>> GetListadoPeliculas(String name, int? genre, String order)
        {
            if (name == null && genre == null && order == null)
            {
                var listadoPeliculas = await applicationDbContext.Peliculas.Select(p =>
            new ViewPeliculaListadoDTO
            {
                Titulo = p.Titulo,
                Lanzamiento = p.Lanzamiento,
                ImagenUrl = p.ImagenUrl
            }).ToListAsync();

                return Ok(listadoPeliculas);
            }
            else {
                /*
                 
                  Punto 10. Búsqueda de Películas o Series

                  Deberá permitir buscar por título, y filtrar por género. Además, permitir ordenar los resultados por fecha de creación de forma ascendiente o descendiente.

                  El término de búsqueda, filtro u ordenación se deberán especificar como parámetros de query:

                    ● /movies?name=nombre
                    ● /movies?genre=idGenero
                    ● /movies?order=ASC | DESC

                */
                var detallePeliculas = await applicationDbContext.Peliculas.Select(p =>
                new ViewPeliculaDetalleDTO
                {
                    Id = p.Id,
                    Titulo = p.Titulo,
                    Lanzamiento = p.Lanzamiento,
                    Calificacion = p.Calificacion,
                    Personajes = p.Personajes,
                    ImagenUrl = p.ImagenUrl,
                    Genero = p.Genero
                }).ToListAsync();
                if (name != null)
                {
                    detallePeliculas = detallePeliculas.Where(p => p.Titulo.Contains(name)).ToList();
                }
                if (genre != null)
                {
                    detallePeliculas = detallePeliculas.Where(p => p.Genero.Id == genre).ToList();
                }
                if (order != null)
                {
                    if (order == "ASC")
                    {
                        detallePeliculas = detallePeliculas.OrderBy(p => p.Lanzamiento).ToList();
                    }
                    if (order =="DESC")
                    {
                        detallePeliculas = detallePeliculas.OrderByDescending(p => p.Lanzamiento).ToList();
                    }
                }

                return Ok(detallePeliculas);
            }
        }

         /*
          8. Detalle de Película / Serie con sus personajes

          Devolverá todos los campos de la película o serie junto a los personajes asociados a la misma
         */

        [HttpGet("moviesDetail")] 
        public async Task<ActionResult<List<ViewPeliculaDetalleDTO>>> GetDetallePersonajes()
        {
            var detallePeliculas = await applicationDbContext.Peliculas.Select(p =>
            new ViewPeliculaDetalleDTO
            {
                Id =p.Id,
                Titulo = p.Titulo,
                Lanzamiento = p.Lanzamiento,
                Calificacion = p.Calificacion,
                Personajes = p.Personajes,
                ImagenUrl = p.ImagenUrl,
                Genero=p.Genero
            }).ToListAsync();
            return Ok(detallePeliculas);
        }

        [HttpPost]
        public override async Task<ActionResult> Post(PeliculaCreationDTO peliculaCreationDTO)
        {
            bool flag = true;
            List<int> generos   = await applicationDbContext.Generos.Select(p=>p.Id).ToListAsync();
            foreach ( int p in generos)
            {
                if (p == peliculaCreationDTO.GeneroId)
                {
                    flag = false;
                }
            }

            if (flag)
            {
                return Problem("El Id de genero ingresado no corresponde a ningun genero existente de la Base de Datos");
            }
            var pelicula = mapper.Map<Pelicula>(peliculaCreationDTO);
            
            if (((IImagen)peliculaCreationDTO).ImagenUrl != null)
            {
                String imagenUrl = await GuardarFoto(((IImagen)peliculaCreationDTO).ImagenUrl);
                pelicula.ImagenUrl = imagenUrl;
            }
            pelicula.Personajes = new List<Personaje>();
            for (int i = 0; i < peliculaCreationDTO.ProtagonistasIds.Length; i++) 
            {
                var personaje = await applicationDbContext.Set<Personaje>().FirstOrDefaultAsync(c => c.Id == peliculaCreationDTO.ProtagonistasIds[i]);
                pelicula.Personajes.Add(personaje);
            }
            var genero = await applicationDbContext.Set<Genero>().FirstOrDefaultAsync(c => c.Id == peliculaCreationDTO.GeneroId);
            pelicula.Genero = new Genero();
            pelicula.Genero = genero;
            applicationDbContext.Add(pelicula);
            await applicationDbContext.SaveChangesAsync();
            var dto = mapper.Map<PeliculaDTO>(pelicula);
            return Ok(dto);
        }

        [HttpPut("{id}")]
        public override async Task<ActionResult> Put(int id, PeliculaCreationDTO peliculaCreationDTO)
        {
            var pelicula = await applicationDbContext.Set<Pelicula>().FirstOrDefaultAsync(c => c.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            if (!String.IsNullOrEmpty(pelicula.ImagenUrl))
            {
                await almacenadorArchivos.Borrar(pelicula.ImagenUrl, ConstantesDeApplicacion.ContenedoresDeArchivos.ContenedorDeImagenes);
            }
            
            applicationDbContext.Entry(pelicula).State = EntityState.Deleted;
            await applicationDbContext.SaveChangesAsync();

            pelicula = mapper.Map<Pelicula>(peliculaCreationDTO);
            if (((IImagen)peliculaCreationDTO).ImagenUrl != null)
            {
                String imagenUrl = await GuardarFoto(((IImagen)peliculaCreationDTO).ImagenUrl);
                pelicula.ImagenUrl = imagenUrl;
            }
            pelicula.Personajes = new List<Personaje>();
            for (int i = 0; i < peliculaCreationDTO.ProtagonistasIds.Length; i++)
            {
                var personaje = await applicationDbContext.Set<Personaje>().FirstOrDefaultAsync(c => c.Id == peliculaCreationDTO.ProtagonistasIds[i]);
                pelicula.Personajes.Add(personaje);
            }
            applicationDbContext.Add(pelicula);
            await applicationDbContext.SaveChangesAsync();
            var dto = mapper.Map<PeliculaDTO>(pelicula);
            /*
            mapper.Map(peliculaCreationDTO, pelicula);

            if (((IImagen)peliculaCreationDTO).ImagenUrl != null)
            {
                if (!String.IsNullOrEmpty(pelicula.ImagenUrl))
                {
                    await almacenadorArchivos.Borrar(pelicula.ImagenUrl, ConstantesDeApplicacion.ContenedoresDeArchivos.ContenedorDeImagenes);
                }
                String imagenUrl = await GuardarFoto(((IImagen)peliculaCreationDTO).ImagenUrl);
                pelicula.ImagenUrl = imagenUrl;
            }
            
            
            pelicula.Personajes = new List<Personaje>();
            
            for (int i = 0; i < peliculaCreationDTO.Protagonistas.Length; i++)
            {
                var personaje = await applicationDbContext.Set<Personaje>().FirstOrDefaultAsync(c => c.Id == peliculaCreationDTO.Protagonistas[i]);
                pelicula.Personajes.Add(personaje);
            }
            applicationDbContext.Entry(pelicula).State = EntityState.Modified;
            await applicationDbContext.SaveChangesAsync();
            
            */
            return NoContent();
        }

    }
}
