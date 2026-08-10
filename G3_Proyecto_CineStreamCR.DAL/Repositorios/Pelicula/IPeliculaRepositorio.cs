using G3_Proyecto_CineStreamCR.DAL.Entidades;

namespace G3_Proyecto_CineStreamCR.DAL.Repositorios.Pelicula
{
    public interface IPeliculaRepositorio
    {
        // Obtiene una página de películas aplicando búsqueda por título,
        // filtro por género, filtro por año y ordenamiento.
        Task<(List<Entidades.Pelicula> Items, int Total)> ObtenerCatalogoAsync(
            string? busqueda,
            int? idGenero,
            int? anio,
            string ordenarPor,
            string direccion,
            int pagina,
            int tamanoPagina);

        // Géneros existentes, usados para el dropdown de filtro del catálogo.
        Task<List<Genero>> ObtenerGenerosAsync();

        // Años en los que existen películas registradas.
        Task<List<int>> ObtenerAniosDisponiblesAsync();
    }
}
