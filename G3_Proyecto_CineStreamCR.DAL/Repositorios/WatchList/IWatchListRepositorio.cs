namespace G3_Proyecto_CineStreamCR.DAL.Repositorios.WatchList
{
    // Integración mínima necesaria para que el catálogo y el detalle de
    // película puedan agregar/quitar una película de una WatchList.
    // La gestión completa de WatchLists (crear, renombrar, eliminar listas,
    // listar todas las listas de un usuario) pertenece a su propio módulo.
    public interface IWatchListRepositorio
    {
        // Obtiene (o crea si no existe) la lista rápida predeterminada del usuario.
        Task<Entidades.WatchList> ObtenerOCrearListaPredeterminadaAsync(int idUsuario);

        // Ids de todas las películas que el usuario ya tiene en alguna WatchList.
        Task<HashSet<int>> ObtenerIdsPeliculasAsync(int idUsuario);

        Task<bool> ExistePeliculaEnListaAsync(int idWatchList, int idPelicula);

        Task<bool> AgregarPeliculaAsync(int idWatchList, int idPelicula);

        Task<bool> QuitarPeliculaAsync(int idWatchList, int idPelicula);
    }
}
