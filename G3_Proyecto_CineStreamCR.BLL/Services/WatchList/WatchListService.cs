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

            if (yaExiste)
            {
                exitoso = await _watchListRepositorio.QuitarPeliculaAsync(lista.IdWatchList, idPelicula);
                enWatchList = false;
            }
            else
            {
                exitoso = await _watchListRepositorio.AgregarPeliculaAsync(lista.IdWatchList, idPelicula);
                enWatchList = true;
            }

            return new ToggleWatchListResultDto
            {
                Exitoso = exitoso,
                EnWatchList = enWatchList,
                Mensaje = exitoso
                    ? (enWatchList ? "Película agregada a tu lista." : "Película eliminada de tu lista.")
                    : "No se pudo actualizar tu WatchList."
            };
        }
    }
}
