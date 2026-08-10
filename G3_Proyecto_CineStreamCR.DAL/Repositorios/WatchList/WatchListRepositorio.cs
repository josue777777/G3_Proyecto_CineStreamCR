using G3_Proyecto_CineStreamCR.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace G3_Proyecto_CineStreamCR.DAL.Repositorios.WatchList
{
    public class WatchListRepositorio : IWatchListRepositorio
    {
        // Nombre de la lista rápida utilizada desde el catálogo y el detalle.
        private const string NombreListaPredeterminada = "Mi Lista";

        private readonly ApplicationDbContext _context;

        public WatchListRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Entidades.WatchList> ObtenerOCrearListaPredeterminadaAsync(int idUsuario)
        {
            var lista = await _context.WatchLists
                .FirstOrDefaultAsync(w => w.IdUsuario == idUsuario && w.Nombre == NombreListaPredeterminada);

            if (lista != null)
            {
                return lista;
            }

            lista = new Entidades.WatchList
            {
                IdUsuario = idUsuario,
                Nombre = NombreListaPredeterminada,
                Descripcion = "Películas guardadas para ver más tarde."
            };

            await _context.WatchLists.AddAsync(lista);
            await _context.SaveChangesAsync();

            return lista;
        }

        public async Task<HashSet<int>> ObtenerIdsPeliculasAsync(int idUsuario)
        {
            var ids = await _context.WatchListPeliculas
                .AsNoTracking()
                .Where(wp => wp.WatchList.IdUsuario == idUsuario)
                .Select(wp => wp.IdPelicula)
                .ToListAsync();

            return ids.ToHashSet();
        }

        public async Task<bool> ExistePeliculaEnListaAsync(int idWatchList, int idPelicula)
        {
            return await _context.WatchListPeliculas
                .AsNoTracking()
                .AnyAsync(wp => wp.IdWatchList == idWatchList && wp.IdPelicula == idPelicula);
        }

        public async Task<bool> AgregarPeliculaAsync(int idWatchList, int idPelicula)
        {
            var existe = await ExistePeliculaEnListaAsync(idWatchList, idPelicula);
            if (existe)
            {
                return true;
            }

            await _context.WatchListPeliculas.AddAsync(new Entidades.WatchListPelicula
            {
                IdWatchList = idWatchList,
                IdPelicula = idPelicula
            });

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> QuitarPeliculaAsync(int idWatchList, int idPelicula)
        {
            var relacion = await _context.WatchListPeliculas
                .FirstOrDefaultAsync(wp => wp.IdWatchList == idWatchList && wp.IdPelicula == idPelicula);

            if (relacion == null)
            {
                return true;
            }

            _context.WatchListPeliculas.Remove(relacion);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
