using G3_Proyecto_CineStreamCR.BLL.Dtos.Personas;
using G3_Proyecto_CineStreamCR.DAL.Repositorios.Persona;

namespace G3_Proyecto_CineStreamCR.BLL.Services.Persona
{
    public class PersonaService : IPersonaService
    {
        private readonly IPersonaRepositorio _personaRepositorio;

        public PersonaService(IPersonaRepositorio personaRepositorio)
        {
            _personaRepositorio = personaRepositorio;
        }

        public async Task<PersonaDetalleDto?> ObtenerDetalleAsync(int idPersona)
        {
            var persona = await _personaRepositorio.ObtenerDetalleAsync(idPersona);
            if (persona == null)
            {
                return null;
            }

            return new PersonaDetalleDto
            {
                IdPersona = persona.IdPersona,
                Nombre = persona.Nombre,
                Nacionalidad = persona.Nacionalidad,
                Biografia = persona.Biografia,
                FechaNacimiento = persona.FechaNacimiento,
                FotoUrl = persona.FotoUrl,
                PeliculasComoDirector = persona.PeliculasDirigidas
                    .Select(p => new PersonaFilmografiaItemDto
                    {
                        IdPelicula = p.IdPelicula,
                        Titulo = p.Titulo,
                        Anio = p.Anio,
                        PosterUrl = p.PosterUrl
                    })
                    .OrderByDescending(p => p.Anio)
                    .ToList(),
                PeliculasComoActor = persona.PeliculasComoActor
                    .Select(pa => new PersonaFilmografiaItemDto
                    {
                        IdPelicula = pa.Pelicula.IdPelicula,
                        Titulo = pa.Pelicula.Titulo,
                        Anio = pa.Pelicula.Anio,
                        PosterUrl = pa.Pelicula.PosterUrl,
                        Personaje = pa.Personaje
                    })
                    .OrderByDescending(p => p.Anio)
                    .ToList()
            };
        }
    }
}
