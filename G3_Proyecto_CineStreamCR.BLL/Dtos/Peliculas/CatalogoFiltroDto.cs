namespace G3_Proyecto_CineStreamCR.BLL.Dtos.Peliculas
{
    // Criterios de búsqueda, filtro, orden y paginación aplicados
    // al catálogo de películas. Se usa tanto para el binding desde
    // la query string (GET) como para pasar los filtros al servicio.
    public class CatalogoFiltroDto
    {
        public string? Busqueda { get; set; }

        public int? IdGenero { get; set; }

        public int? Anio { get; set; }

        // Valores válidos: "titulo", "anio", "rating".
        public string OrdenarPor { get; set; } = "titulo";

        // Valores válidos: "asc", "desc".
        public string Direccion { get; set; } = "asc";

        public int Pagina { get; set; } = 1;

        public int TamanoPagina { get; set; } = 12;
    }
}
