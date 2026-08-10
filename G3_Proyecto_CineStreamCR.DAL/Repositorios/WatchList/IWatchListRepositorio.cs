namespace G3_Proyecto_CineStreamCR.DAL.Repositorios.WatchList
{
    public interface IWatchListRepositorio
    {
        Task<List<Entidades.WatchList>> ObtenerPorUsuarioAsync(int idUsuario);

        Task<Entidades.WatchList?> ObtenerDetalleAsync(int idWatchList, int idUsuario);

        Task<Entidades.WatchList?> ObtenerPorIdAsync(int idWatchList, int idUsuario);

        Task<Entidades.Pelicula?> ObtenerPeliculaAsync(int idPelicula);

        Task<bool> ExisteNombreAsync(int idUsuario, string nombre, int? idWatchListExcluir = null);

        Task<bool> CrearAsync(Entidades.WatchList watchList);

        Task<bool> EditarAsync(Entidades.WatchList watchList);

        Task<bool> EliminarAsync(Entidades.WatchList watchList);

        Task<Entidades.WatchList> ObtenerOCrearListaPredeterminadaAsync(int idUsuario);

        Task<HashSet<int>> ObtenerIdsPeliculasAsync(int idUsuario);

        Task<bool> ExistePeliculaEnListaAsync(int idWatchList, int idPelicula);

        Task<bool> AgregarPeliculaAsync(int idWatchList, int idPelicula);

        Task<bool> QuitarPeliculaAsync(int idWatchList, int idPelicula);

        Task<bool> GuardarSeleccionAsync(
            int idUsuario,
            int idPelicula,
            IEnumerable<int> idsWatchLists);
    }
}
