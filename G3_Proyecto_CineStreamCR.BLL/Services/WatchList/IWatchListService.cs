using G3_Proyecto_CineStreamCR.BLL.Dtos.WatchLists;

namespace G3_Proyecto_CineStreamCR.BLL.Services.WatchList
{
    public interface IWatchListService
    {
        Task<List<WatchListResumenDto>> ObtenerListasAsync(int idUsuario);

        Task<WatchListDetalleDto?> ObtenerDetalleAsync(int idUsuario, int idWatchList);

        Task<WatchListFormDto?> ObtenerFormularioAsync(int idUsuario, int idWatchList);

        Task<WatchListOperacionResultDto> CrearAsync(int idUsuario, WatchListFormDto model);

        Task<WatchListOperacionResultDto> EditarAsync(int idUsuario, WatchListFormDto model);

        Task<WatchListOperacionResultDto> EliminarAsync(int idUsuario, int idWatchList);

        Task<WatchListOperacionResultDto> QuitarPeliculaAsync(
            int idUsuario,
            int idWatchList,
            int idPelicula);

        Task<WatchListSeleccionDto?> ObtenerSeleccionAsync(int idUsuario, int idPelicula);

        Task<WatchListOperacionResultDto> GuardarSeleccionAsync(
            int idUsuario,
            WatchListSeleccionDto model);

        Task<HashSet<int>> ObtenerIdsPeliculasAsync(int idUsuario);

        Task<bool> EstaEnWatchListAsync(int idUsuario, int idPelicula);

        Task<ToggleWatchListResultDto> ToggleAsync(int idUsuario, int idPelicula);
    }
}
