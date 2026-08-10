namespace G3_Proyecto_CineStreamCR.BLL.Dtos.Peliculas
{
    // Página de resultados del catálogo junto con la información
    // necesaria para construir la navegación entre páginas.
    public class CatalogoResultadoDto
    {
        public List<PeliculaCatalogoDto> Items { get; set; } = new();

        public int PaginaActual { get; set; } = 1;

        public int TamanoPagina { get; set; } = 12;

        public int TotalRegistros { get; set; }

        public int TotalPaginas { get; set; } = 1;
    }
}
