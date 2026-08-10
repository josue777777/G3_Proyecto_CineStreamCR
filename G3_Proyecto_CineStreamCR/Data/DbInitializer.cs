using G3_Proyecto_CineStreamCR.DAL.Data;
using G3_Proyecto_CineStreamCR.DAL.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G3_Proyecto_CineStreamCR.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Aseguramos que la base de datos esté creada y tenga las migraciones aplicadas.
            await context.Database.MigrateAsync();

            // 1. Semillero de Personas (Directores y Actores)
            if (!await context.Personas.AnyAsync())
            {
                var personas = new List<Persona>
                {
                    new Persona { Nombre = "Christopher Nolan", Nacionalidad = "Británico/Estadounidense", Biografia = "Director, guionista y productor conocido por películas de gran escala y narrativa compleja.", FechaNacimiento = new DateTime(1970, 7, 30), FotoUrl = "/images/nolan.jpg" },
                    new Persona { Nombre = "Denis Villeneuve", Nacionalidad = "Canadiense", Biografia = "Director de cine y escritor canadiense, aclamado por su trabajo en ciencia ficción y drama.", FechaNacimiento = new DateTime(1967, 10, 3), FotoUrl = "/images/villeneuve.jpg" },
                    new Persona { Nombre = "Timothée Chalamet", Nacionalidad = "Estadounidense/Francés", Biografia = "Actor nominado al Óscar, conocido por sus papeles en Dune, Call Me by Your Name y Wonka.", FechaNacimiento = new DateTime(1995, 12, 27), FotoUrl = "/images/chalamet.jpg" },
                    new Persona { Nombre = "Cillian Murphy", Nacionalidad = "Irlandés", Biografia = "Actor ganador del Óscar, célebre por Oppenheimer, Peaky Blinders e Inception.", FechaNacimiento = new DateTime(1976, 5, 25), FotoUrl = "/images/murphy.jpg" }
                };

                await context.Personas.AddRangeAsync(personas);
                await context.SaveChangesAsync();
            }

            // 2. Semillero de Géneros
            if (!await context.Generos.AnyAsync())
            {
                var generos = new List<Genero>
                {
                    new Genero { Nombre = "Acción" },
                    new Genero { Nombre = "Ciencia Ficción" },
                    new Genero { Nombre = "Drama" },
                    new Genero { Nombre = "Suspenso" },
                    new Genero { Nombre = "Aventura" }
                };

                await context.Generos.AddRangeAsync(generos);
                await context.SaveChangesAsync();
            }

            // 3. Semillero de Películas
            if (!await context.Peliculas.AnyAsync())
            {
                var nolan = await context.Personas.FirstAsync(p => p.Nombre == "Christopher Nolan");
                var villeneuve = await context.Personas.FirstAsync(p => p.Nombre == "Denis Villeneuve");

                var peliculas = new List<Pelicula>
                {
                    new Pelicula
                    {
                        Titulo = "Dune: Part Two",
                        Sinopsis = "Paul Atreides se une a Chani y a los Fremen mientras busca venganza contra los conspiradores que destruyeron a su familia. Enfrentando una elección entre el amor de su vida y el destino del universo, se esfuerza por evitar un futuro terrible que solo él puede prever.",
                        Anio = 2024,
                        DuracionMinutos = 166,
                        PosterUrl = "https://images.unsplash.com/photo-1534447677768-be436bb09401?w=500&auto=format&fit=crop&q=60", // Imagen espacial simulando poster
                        VideoUrl = "/videos/trailer_dummy.mp4",
                        Rating = 0,
                        IdDirector = villeneuve.IdPersona
                    },
                    new Pelicula
                    {
                        Titulo = "Interstellar",
                        Sinopsis = "Un equipo de exploradores viaja a través de un agujero de gusano en el espacio en un intento por asegurar la supervivencia de la humanidad ante una plaga global en la Tierra.",
                        Anio = 2014,
                        DuracionMinutos = 169,
                        PosterUrl = "https://images.unsplash.com/photo-1451187580459-43490279c0fa?w=500&auto=format&fit=crop&q=60", // Imagen galaxia simulando poster
                        VideoUrl = "/videos/trailer_dummy.mp4",
                        Rating = 0,
                        IdDirector = nolan.IdPersona
                    },
                    new Pelicula
                    {
                        Titulo = "Inception",
                        Sinopsis = "A un ladrón que roba secretos corporativos a través del uso de la tecnología de compartir sueños se le da la tarea inversa de plantar una idea en la mente de un director general.",
                        Anio = 2010,
                        DuracionMinutos = 148,
                        PosterUrl = "https://images.unsplash.com/photo-1509198397868-475647b2a1e5?w=500&auto=format&fit=crop&q=60", // Imagen abstracta/sueños
                        VideoUrl = "/videos/trailer_dummy.mp4",
                        Rating = 0,
                        IdDirector = nolan.IdPersona
                    }
                };

                await context.Peliculas.AddRangeAsync(peliculas);
                await context.SaveChangesAsync();

                // Relacionar Película y Géneros
                var scifi = await context.Generos.FirstAsync(g => g.Nombre == "Ciencia Ficción");
                var aventura = await context.Generos.FirstAsync(g => g.Nombre == "Aventura");
                var accion = await context.Generos.FirstAsync(g => g.Nombre == "Acción");
                var drama = await context.Generos.FirstAsync(g => g.Nombre == "Drama");

                var dune2 = peliculas[0];
                var interstellar = peliculas[1];
                var inception = peliculas[2];

                await context.PeliculaGeneros.AddRangeAsync(
                    new PeliculaGenero { IdPelicula = dune2.IdPelicula, IdGenero = scifi.IdGenero },
                    new PeliculaGenero { IdPelicula = dune2.IdPelicula, IdGenero = aventura.IdGenero },
                    new PeliculaGenero { IdPelicula = interstellar.IdPelicula, IdGenero = scifi.IdGenero },
                    new PeliculaGenero { IdPelicula = interstellar.IdPelicula, IdGenero = drama.IdGenero },
                    new PeliculaGenero { IdPelicula = inception.IdPelicula, IdGenero = scifi.IdGenero },
                    new PeliculaGenero { IdPelicula = inception.IdPelicula, IdGenero = accion.IdGenero }
                );

                // Relacionar Película y Actores
                var chalamet = await context.Personas.FirstAsync(p => p.Nombre == "Timothée Chalamet");
                var murphy = await context.Personas.FirstAsync(p => p.Nombre == "Cillian Murphy");

                await context.PeliculaActores.AddRangeAsync(
                    new PeliculaActor { IdPelicula = dune2.IdPelicula, IdActor = chalamet.IdPersona, Personaje = "Paul Atreides" },
                    new PeliculaActor { IdPelicula = inception.IdPelicula, IdActor = murphy.IdPersona, Personaje = "Robert Fischer" }
                );

                await context.SaveChangesAsync();
            }

            // 4. Semillero de Usuarios (Usuario demo)
            if (!await context.Usuarios.AnyAsync())
            {
                var hasher = new PasswordHasher<Usuario>();
                var demoUser = new Usuario
                {
                    NombreUsuario = "demo",
                    Correo = "demo@cinestream.cr",
                };
                demoUser.PasswordHash = hasher.HashPassword(demoUser, "Password123!");

                await context.Usuarios.AddAsync(demoUser);
                await context.SaveChangesAsync();
            }
        }
    }
}
