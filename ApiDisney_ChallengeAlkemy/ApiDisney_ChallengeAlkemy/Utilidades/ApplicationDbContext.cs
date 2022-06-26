using ApiDisney_ChallengeAlkemy.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ApiDisney_ChallengeAlkemy.Utilidades
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
            : base(dbContextOptions)
        {

        }
               
        public DbSet<Personaje> Personajes { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<Genero> Generos { get; set; }

    }
}
