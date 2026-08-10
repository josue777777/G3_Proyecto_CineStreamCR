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

        public async Task<List<Entidades.WatchList>> ObtenerPorUsuarioAsync(int idUsuario)
        {
            return await _context.WatchLists
                .AsNoTracking()
                .Include(w => w.WatchListPeliculas)
                .Where(w => w.IdUsuario == idUsuario)
                .OrderBy(w => w.Nombre)
                .ToListAsync();
        }

        public async Task<Entidades.WatchList?> ObtenerDetalleAsync(int idWatchList, int idUsuario)
        {
            return await _context.WatchLists
                .AsNoTracking()
                .Include(w => w.WatchListPeliculas)
                    .ThenInclude(wp => wp.Pelicula)
                .FirstOrDefaultAsync(w =>
                    w.IdWatchList == idWatchList &&
                    w.IdUsuario == idUsuario);
        }

        public async Task<Entidades.WatchList?> ObtenerPorIdAsync(int idWatchList, int idUsuario)
        {
            return await _context.WatchLists
                .FirstOrDefaultAsync(w =>
                    w.IdWatchList == idWatchList &&
                    w.IdUsuario == idUsuario);
        }

        public async Task<Entidades.Pelicula?> ObtenerPeliculaAsync(int idPelicula)
        {
            return await _context.Peliculas
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdPelicula == idPelicula);
        }

        public async Task<bool> ExisteNombreAsync(
            int idUsuario,
            string nombre,
            int? idWatchListExcluir = null)
        {
            return await _context.WatchLists
                .AsNoTracking()
                .AnyAsync(w =>
                    w.IdUsuario == idUsuario &&
                    w.Nombre == nombre &&
                    (!idWatchListExcluir.HasValue || w.IdWatchList != idWatchListExcluir.Value));
        }

        public async Task<bool> CrearAsync(Entidades.WatchList watchList)
        {
            await _context.WatchLists.AddAsync(watchList);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EditarAsync(Entidades.WatchList watchList)
        {
            _context.WatchLists.Update(watchList);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarAsync(Entidades.WatchList watchList)
        {
            _context.WatchLists.Remove(watchList);
            return await _context.SaveChangesAsync() > 0;
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

        public async Task<bool> GuardarSeleccionAsync(
            int idUsuario,
            int idPelicula,
            IEnumerable<int> idsWatchLists)
        {
            var idsListasUsuario = await _context.WatchLists
                .Where(w => w.IdUsuario == idUsuario)
                .Select(w => w.IdWatchList)
                .ToListAsync();

            var idsSeleccionados = idsWatchLists
                .Where(id => idsListasUsuario.Contains(id))
                .ToHashSet();

            var relacionesActuales = await _context.WatchListPeliculas
                .Where(wp =>
                    wp.IdPelicula == idPelicula &&
                    idsListasUsuario.Contains(wp.IdWatchList))
                .ToListAsync();

            var relacionesAEliminar = relacionesActuales
                .Where(wp => !idsSeleccionados.Contains(wp.IdWatchList))
                .ToList();

            _context.WatchListPeliculas.RemoveRange(relacionesAEliminar);

            var idsActuales = relacionesActuales
                .Select(wp => wp.IdWatchList)
                .ToHashSet();

            var relacionesAAgregar = idsSeleccionados
                .Where(id => !idsActuales.Contains(id))
                .Select(id => new Entidades.WatchListPelicula
                {
                    IdWatchList = id,
                    IdPelicula = idPelicula
                });

            await _context.WatchListPeliculas.AddRangeAsync(relacionesAAgregar);

            if (!relacionesAEliminar.Any() && !idsSeleccionados.Any(id => !idsActuales.Contains(id)))
            {
                return true;
            }

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
