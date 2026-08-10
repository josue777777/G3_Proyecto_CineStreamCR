using G3_Proyecto_CineStreamCR.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace G3_Proyecto_CineStreamCR.DAL.Repositorios.Persona
{
    public class PersonaRepositorio : IPersonaRepositorio
    {
        private readonly ApplicationDbContext _context;

        public PersonaRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Entidades.Persona>> ObtenerTodasAsync()
        {
            return await _context.Personas
                .AsNoTracking()
                .Include(p => p.PeliculasDirigidas)
                .Include(p => p.PeliculasComoActor)
                .ToListAsync();
        }

        public async Task<Entidades.Persona?> ObtenerDetalleAsync(int idPersona)
        {
            return await _context.Personas
                .AsNoTracking()
                .Include(p => p.PeliculasDirigidas)
                .Include(p => p.PeliculasComoActor)
                    .ThenInclude(pa => pa.Pelicula)
                .FirstOrDefaultAsync(p => p.IdPersona == idPersona);
        }
    }
}
