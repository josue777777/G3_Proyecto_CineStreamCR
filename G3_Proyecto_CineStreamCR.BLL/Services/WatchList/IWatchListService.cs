using G3_Proyecto_CineStreamCR.BLL.Dtos.WatchLists;

namespace G3_Proyecto_CineStreamCR.BLL.Services.WatchList
{
    // Integración mínima de WatchList necesaria para el catálogo y el
    // detalle de película. La gestión completa de listas (crear, renombrar,
    // eliminar, listar todas las listas de un usuario) pertenece a su propio módulo.
    public interface IWatchListService
    {
        Task<HashSet<int>> ObtenerIdsPeliculasAsync(int idUsuario);

        Task<bool> EstaEnWatchListAsync(int idUsuario, int idPelicula);

        Task<ToggleWatchListResultDto> ToggleAsync(int idUsuario, int idPelicula);
    }
}
