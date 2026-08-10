using G3_Proyecto_CineStreamCR.BLL.Dtos.WatchLists;
using G3_Proyecto_CineStreamCR.DAL.Repositorios.WatchList;

namespace G3_Proyecto_CineStreamCR.BLL.Services.WatchList
{
    public class WatchListService : IWatchListService
    {
        private readonly IWatchListRepositorio _watchListRepositorio;

        public WatchListService(IWatchListRepositorio watchListRepositorio)
        {
            _watchListRepositorio = watchListRepositorio;
        }

        public async Task<List<WatchListResumenDto>> ObtenerListasAsync(int idUsuario)
        {
            var listas = await _watchListRepositorio.ObtenerPorUsuarioAsync(idUsuario);

            return listas.Select(lista => new WatchListResumenDto
            {
                IdWatchList = lista.IdWatchList,
                Nombre = lista.Nombre,
                Descripcion = lista.Descripcion,
                CantidadPeliculas = lista.WatchListPeliculas.Count
            }).ToList();
        }

        public async Task<WatchListDetalleDto?> ObtenerDetalleAsync(int idUsuario, int idWatchList)
        {
            var lista = await _watchListRepositorio.ObtenerDetalleAsync(idWatchList, idUsuario);
            if (lista == null)
            {
                return null;
            }

            return new WatchListDetalleDto
            {
                IdWatchList = lista.IdWatchList,
                Nombre = lista.Nombre,
                Descripcion = lista.Descripcion,
                Peliculas = lista.WatchListPeliculas
                    .Select(wp => new WatchListPeliculaDto
                    {
                        IdPelicula = wp.Pelicula.IdPelicula,
                        Titulo = wp.Pelicula.Titulo,
                        PosterUrl = wp.Pelicula.PosterUrl,
                        Anio = wp.Pelicula.Anio,
                        DuracionMinutos = wp.Pelicula.DuracionMinutos,
                        Rating = wp.Pelicula.Rating
                    })
                    .OrderBy(p => p.Titulo)
                    .ToList()
            };
        }

        public async Task<WatchListFormDto?> ObtenerFormularioAsync(int idUsuario, int idWatchList)
        {
            var lista = await _watchListRepositorio.ObtenerPorIdAsync(idWatchList, idUsuario);
            if (lista == null)
            {
                return null;
            }

            return new WatchListFormDto
            {
                IdWatchList = lista.IdWatchList,
                Nombre = lista.Nombre,
                Descripcion = lista.Descripcion
            };
        }

        public async Task<WatchListOperacionResultDto> CrearAsync(
            int idUsuario,
            WatchListFormDto model)
        {
            var nombre = model.Nombre.Trim();

            if (await _watchListRepositorio.ExisteNombreAsync(idUsuario, nombre))
            {
                return Resultado(false, "Ya existe una lista con ese nombre.");
            }

            var lista = new G3_Proyecto_CineStreamCR.DAL.Entidades.WatchList
            {
                IdUsuario = idUsuario,
                Nombre = nombre,
                Descripcion = LimpiarDescripcion(model.Descripcion)
            };

            var exitoso = await _watchListRepositorio.CrearAsync(lista);
            return Resultado(
                exitoso,
                exitoso ? "La lista se creó correctamente." : "No se pudo crear la lista.");
        }

        public async Task<WatchListOperacionResultDto> EditarAsync(
            int idUsuario,
            WatchListFormDto model)
        {
            var lista = await _watchListRepositorio.ObtenerPorIdAsync(model.IdWatchList, idUsuario);
            if (lista == null)
            {
                return Resultado(false, "La lista no existe.");
            }

            var nombre = model.Nombre.Trim();
            var nombreRepetido = await _watchListRepositorio.ExisteNombreAsync(
                idUsuario,
                nombre,
                model.IdWatchList);

            if (nombreRepetido)
            {
                return Resultado(false, "Ya existe otra lista con ese nombre.");
            }

            lista.Nombre = nombre;
            lista.Descripcion = LimpiarDescripcion(model.Descripcion);

            var exitoso = await _watchListRepositorio.EditarAsync(lista);
            return Resultado(
                exitoso,
                exitoso ? "La lista se actualizó correctamente." : "No se pudo actualizar la lista.");
        }

        public async Task<WatchListOperacionResultDto> EliminarAsync(int idUsuario, int idWatchList)
        {
            var lista = await _watchListRepositorio.ObtenerPorIdAsync(idWatchList, idUsuario);
            if (lista == null)
            {
                return Resultado(false, "La lista no existe.");
            }

            var exitoso = await _watchListRepositorio.EliminarAsync(lista);
            return Resultado(
                exitoso,
                exitoso ? "La lista se eliminó correctamente." : "No se pudo eliminar la lista.");
        }

        public async Task<WatchListOperacionResultDto> QuitarPeliculaAsync(
            int idUsuario,
            int idWatchList,
            int idPelicula)
        {
            var lista = await _watchListRepositorio.ObtenerPorIdAsync(idWatchList, idUsuario);
            if (lista == null)
            {
                return Resultado(false, "La lista no existe.");
            }

            var exitoso = await _watchListRepositorio.QuitarPeliculaAsync(idWatchList, idPelicula);
            return Resultado(
                exitoso,
                exitoso ? "La película se quitó de la lista." : "No se pudo quitar la película.");
        }

        public async Task<WatchListSeleccionDto?> ObtenerSeleccionAsync(
            int idUsuario,
            int idPelicula)
        {
            var pelicula = await _watchListRepositorio.ObtenerPeliculaAsync(idPelicula);
            if (pelicula == null)
            {
                return null;
            }

            var listas = await _watchListRepositorio.ObtenerPorUsuarioAsync(idUsuario);

            return new WatchListSeleccionDto
            {
                IdPelicula = pelicula.IdPelicula,
                TituloPelicula = pelicula.Titulo,
                Listas = listas.Select(lista => new WatchListSeleccionItemDto
                {
                    IdWatchList = lista.IdWatchList,
                    Nombre = lista.Nombre,
                    Seleccionada = lista.WatchListPeliculas.Any(wp => wp.IdPelicula == idPelicula)
                }).ToList()
            };
        }

        public async Task<WatchListOperacionResultDto> GuardarSeleccionAsync(
            int idUsuario,
            WatchListSeleccionDto model)
        {
            var pelicula = await _watchListRepositorio.ObtenerPeliculaAsync(model.IdPelicula);
            if (pelicula == null)
            {
                return Resultado(false, "La película no existe.");
            }

            var listasUsuario = await _watchListRepositorio.ObtenerPorUsuarioAsync(idUsuario);
            var idsValidos = listasUsuario.Select(lista => lista.IdWatchList).ToHashSet();
            var idsSeleccionados = model.Listas
                .Where(lista => lista.Seleccionada)
                .Select(lista => lista.IdWatchList)
                .Distinct()
                .ToList();

            if (idsSeleccionados.Any(id => !idsValidos.Contains(id)))
            {
                return Resultado(false, "Una de las listas seleccionadas no es válida.");
            }

            var exitoso = await _watchListRepositorio.GuardarSeleccionAsync(
                idUsuario,
                model.IdPelicula,
                idsSeleccionados);

            return Resultado(
                exitoso,
                exitoso ? "Las listas se actualizaron correctamente." : "No se pudieron actualizar las listas.");
        }

        public Task<HashSet<int>> ObtenerIdsPeliculasAsync(int idUsuario)
            => _watchListRepositorio.ObtenerIdsPeliculasAsync(idUsuario);

        public async Task<bool> EstaEnWatchListAsync(int idUsuario, int idPelicula)
        {
            var ids = await _watchListRepositorio.ObtenerIdsPeliculasAsync(idUsuario);
            return ids.Contains(idPelicula);
        }

        public async Task<ToggleWatchListResultDto> ToggleAsync(int idUsuario, int idPelicula)
        {
            var lista = await _watchListRepositorio.ObtenerOCrearListaPredeterminadaAsync(idUsuario);
            var yaExiste = await _watchListRepositorio.ExistePeliculaEnListaAsync(lista.IdWatchList, idPelicula);

            bool exitoso;
            bool enWatchList;
            bool seAgrego;

            if (yaExiste)
            {
                exitoso = await _watchListRepositorio.QuitarPeliculaAsync(lista.IdWatchList, idPelicula);
                seAgrego = false;
            }
            else
            {
                exitoso = await _watchListRepositorio.AgregarPeliculaAsync(lista.IdWatchList, idPelicula);
                seAgrego = true;
            }

            enWatchList = await EstaEnWatchListAsync(idUsuario, idPelicula);

            return new ToggleWatchListResultDto
            {
                Exitoso = exitoso,
                EnWatchList = enWatchList,
                Mensaje = exitoso
                    ? (seAgrego ? "Película agregada a tu lista." : "Película eliminada de Mi Lista.")
                    : "No se pudo actualizar tu WatchList."
            };
        }

        private static string? LimpiarDescripcion(string? descripcion)
        {
            return string.IsNullOrWhiteSpace(descripcion)
                ? null
                : descripcion.Trim();
        }

        private static WatchListOperacionResultDto Resultado(bool exitoso, string mensaje)
        {
            return new WatchListOperacionResultDto
            {
                Exitoso = exitoso,
                Mensaje = mensaje
            };
        }
    }
}
