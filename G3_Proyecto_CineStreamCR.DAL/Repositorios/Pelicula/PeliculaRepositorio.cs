using G3_Proyecto_CineStreamCR.DAL.Data;
using G3_Proyecto_CineStreamCR.DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace G3_Proyecto_CineStreamCR.DAL.Repositorios.Pelicula
{
    public class PeliculaRepositorio : IPeliculaRepositorio
    {
        private readonly ApplicationDbContext _context;

        public PeliculaRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtiene una página de películas aplicando búsqueda, filtros y orden
        // directamente en la consulta para no traer todo el catálogo a memoria.
        public async Task<(List<Entidades.Pelicula> Items, int Total)> ObtenerCatalogoAsync(
            string? busqueda,
            int? idGenero,
            int? anio,
            string ordenarPor,
            string direccion,
            int pagina,
            int tamanoPagina)
        {
            var query = _context.Peliculas
                .AsNoTracking()
                .Include(p => p.Director)
                .Include(p => p.PeliculaGeneros)
                    .ThenInclude(pg => pg.Genero)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                var termino = busqueda.Trim();
                query = query.Where(p => EF.Functions.Like(p.Titulo, $"%{termino}%"));
            }

            if (idGenero.HasValue)
            {
                query = query.Where(p => p.PeliculaGeneros.Any(pg => pg.IdGenero == idGenero.Value));
            }

            if (anio.HasValue)
            {
                query = query.Where(p => p.Anio == anio.Value);
            }

            var descendente = string.Equals(direccion, "desc", StringComparison.OrdinalIgnoreCase);

            query = ordenarPor.ToLowerInvariant() switch
            {
                "anio" => descendente ? query.OrderByDescending(p => p.Anio) : query.OrderBy(p => p.Anio),
                "rating" => descendente ? query.OrderByDescending(p => p.Rating) : query.OrderBy(p => p.Rating),
                _ => descendente ? query.OrderByDescending(p => p.Titulo) : query.OrderBy(p => p.Titulo),
            };

            var total = await query.CountAsync();

            var items = await query
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();

            return (items, total);
        }

        public async Task<List<Genero>> ObtenerGenerosAsync()
        {
            return await _context.Generos
                .AsNoTracking()
                .OrderBy(g => g.Nombre)
                .ToListAsync();
        }

        public async Task<List<int>> ObtenerAniosDisponiblesAsync()
        {
            return await _context.Peliculas
                .AsNoTracking()
                .Select(p => p.Anio)
                .Distinct()
                .OrderByDescending(a => a)
                .ToListAsync();
        }
    }
}
