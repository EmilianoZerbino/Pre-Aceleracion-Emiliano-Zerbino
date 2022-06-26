using ApiDisney_ChallengeAlkemy.DTOs.GeneroDTOs;
using ApiDisney_ChallengeAlkemy.DTOs.PeliculaDTOs;
using ApiDisney_ChallengeAlkemy.DTOs.PersonajeDTOs;
using ApiDisney_ChallengeAlkemy.Entidades;
using AutoMapper;

namespace BibliotecaAPI.Utilidades
{
    public class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles()
        {

            //---Peliculas--------------------------
            CreateMap<Pelicula, PeliculaDTO>()
                .ReverseMap();
            CreateMap<PeliculaCreationDTO, Pelicula>()
                .ForMember(m => m.ImagenUrl, options => options.Ignore());
           
            //---Personaje--------------------------
            CreateMap<Personaje, PersonajeDTO>()
                 .ReverseMap();
            CreateMap<PersonajeCreationDTO, Personaje>()
                .ForMember(m => m.ImagenUrl, options => options.Ignore());
           
            //---Genero-----------------------------
            CreateMap<Genero, GeneroDTO>()
                 .ReverseMap();
            CreateMap<GeneroCreationDTO, Genero>()
                .ForMember(m => m.ImagenUrl, options => options.Ignore());

        }
    }
}
