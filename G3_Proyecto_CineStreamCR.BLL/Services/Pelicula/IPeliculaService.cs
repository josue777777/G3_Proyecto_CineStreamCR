using G3_Proyecto_CineStreamCR.BLL.Dtos.Peliculas;
using G3_Proyecto_CineStreamCR.DAL.Entidades;

namespace G3_Proyecto_CineStreamCR.BLL.Services.Pelicula
{
    public interface IPeliculaService
    {
        // Catálogo paginado, con búsqueda/filtros/orden ya aplicados
        // y marcando qué películas están en la WatchList del usuario indicado.
        Task<CatalogoResultadoDto> ObtenerCatalogoAsync(CatalogoFiltroDto filtro, int? idUsuarioActual);

        Task<List<Genero>> ObtenerGenerosAsync();

        Task<List<int>> ObtenerAniosDisponiblesAsync();
    }
}
