using ApiDisney_ChallengeAlkemy.Entidades;
using ApiDisney_ChallengeAlkemy.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ApiDisney_ChallengeAlkemy.Controllers.CRUDController
{
    public abstract class BaseController<TCreation, TEntity, TDTO> : ControllerBase
        where TEntity : class, IId
    {

        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;
        private readonly string controllerName;
        private readonly IAlmacenadorArchivos almacenadorArchivos;

        public BaseController(ApplicationDbContext applicationDbContext, IMapper mapper, string controllerName, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
            this.controllerName = controllerName;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public virtual async Task<ActionResult<List<TDTO>>> Get()
        {
            var entidades = await applicationDbContext.Set<TEntity>().ToListAsync();

            return mapper.Map<List<TDTO>>(entidades);
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TDTO>> Get(int id)
        {
            var entidad = await applicationDbContext.Set<TEntity>().FirstOrDefaultAsync(c => c.Id == id);

            if (entidad == null)
            {
                return NotFound();
            }

            return mapper.Map<TDTO>(entidad);
        }

        [HttpPost]
        public virtual async Task<ActionResult> Post([FromForm] TCreation creationDTO)
        {
            var entidad = mapper.Map<TEntity>(creationDTO);
            if (((IImagen)creationDTO).ImagenUrl != null)
            {
                String imagenUrl = await GuardarFoto(((IImagen)creationDTO).ImagenUrl);
                entidad.ImagenUrl = imagenUrl;
            }
            applicationDbContext.Add(entidad);
            await applicationDbContext.SaveChangesAsync();
            var dto = mapper.Map<TDTO>(entidad);
            return Ok(dto);
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult> Put(int id, [FromForm] TCreation creationDTO)
        {
            var entidad = await applicationDbContext.Set<TEntity>().FirstOrDefaultAsync(c => c.Id == id);
            if (entidad == null)
            {
                return NotFound();
            }

            mapper.Map(creationDTO, entidad);

            if (((IImagen)creationDTO).ImagenUrl != null)
            {
                if (!String.IsNullOrEmpty(entidad.ImagenUrl))
                {
                    await almacenadorArchivos.Borrar(entidad.ImagenUrl, ConstantesDeApplicacion.ContenedoresDeArchivos.ContenedorDeImagenes);
                }
                String imagenUrl = await GuardarFoto(((IImagen)creationDTO).ImagenUrl);
                entidad.ImagenUrl = imagenUrl;
            }

            applicationDbContext.Entry(entidad).State = EntityState.Modified;
            await applicationDbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(int id)
        {
            var entidad = await applicationDbContext.Set<TEntity>().FirstOrDefaultAsync(c => c.Id == id);

            if (entidad == null)
            {
                return NotFound();
            }

            if (!String.IsNullOrEmpty(entidad.ImagenUrl))
            {
                await almacenadorArchivos.Borrar(entidad.ImagenUrl, ConstantesDeApplicacion.ContenedoresDeArchivos.ContenedorDeImagenes);
            }

            applicationDbContext.Entry(entidad).State = EntityState.Deleted;
            await applicationDbContext.SaveChangesAsync();
            return NoContent();

        }

        protected async Task<String> GuardarFoto(IFormFile imagen)
        {
            using var stream = new MemoryStream();

            await imagen.CopyToAsync(stream);

            var fileBytes = stream.ToArray();

            return await almacenadorArchivos.Crear(fileBytes, imagen.ContentType, Path.GetExtension(imagen.FileName),
                ConstantesDeApplicacion.ContenedoresDeArchivos.ContenedorDeImagenes, Guid.NewGuid().ToString());


        }
    }
}
