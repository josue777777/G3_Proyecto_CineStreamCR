using G3_Proyecto_CineStreamCR.BLL.Dtos.Peliculas;
using G3_Proyecto_CineStreamCR.DAL.Entidades;

namespace G3_Proyecto_CineStreamCR.Models.Peliculas
{
    // Modelo compuesto para la vista de catálogo: resultado paginado,
    // filtros aplicados y catálogos auxiliares (géneros, años) para los dropdowns.
    public class CatalogoIndexViewModel
    {
        public CatalogoResultadoDto Resultado { get; set; } = new();

        public CatalogoFiltroDto Filtro { get; set; } = new();

        public List<Genero> Generos { get; set; } = new();

        public List<int> Anios { get; set; } = new();
    }
}
