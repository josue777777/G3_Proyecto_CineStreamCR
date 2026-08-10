using G3_Proyecto_CineStreamCR.BLL.Dtos.Peliculas;
using G3_Proyecto_CineStreamCR.BLL.Services.WatchList;
using G3_Proyecto_CineStreamCR.DAL.Entidades;
using G3_Proyecto_CineStreamCR.DAL.Repositorios.Pelicula;

namespace G3_Proyecto_CineStreamCR.BLL.Services.Pelicula
{
    public class PeliculaService : IPeliculaService
    {
        private readonly IPeliculaRepositorio _peliculaRepositorio;
        private readonly IWatchListService _watchListService;

        public PeliculaService(
            IPeliculaRepositorio peliculaRepositorio,
            IWatchListService watchListService)
        {
            _peliculaRepositorio = peliculaRepositorio;
            _watchListService = watchListService;
        }

        public async Task<CatalogoResultadoDto> ObtenerCatalogoAsync(CatalogoFiltroDto filtro, int? idUsuarioActual)
        {
            filtro ??= new CatalogoFiltroDto();

            var pagina = filtro.Pagina < 1 ? 1 : filtro.Pagina;
            var tamanoPagina = filtro.TamanoPagina < 1 ? 12 : filtro.TamanoPagina;
            var ordenarPor = string.IsNullOrWhiteSpace(filtro.OrdenarPor) ? "titulo" : filtro.OrdenarPor;
            var direccion = string.IsNullOrWhiteSpace(filtro.Direccion) ? "asc" : filtro.Direccion;

            var (items, total) = await _peliculaRepositorio.ObtenerCatalogoAsync(
                filtro.Busqueda,
                filtro.IdGenero,
                filtro.Anio,
                ordenarPor,
                direccion,
                pagina,
                tamanoPagina);

            var idsEnWatchList = idUsuarioActual.HasValue
                ? await _watchListService.ObtenerIdsPeliculasAsync(idUsuarioActual.Value)
                : new HashSet<int>();

            var dtoItems = items.Select(p => new PeliculaCatalogoDto
            {
                IdPelicula = p.IdPelicula,
                Titulo = p.Titulo,
                Sinopsis = p.Sinopsis,
                PosterUrl = p.PosterUrl,
                Anio = p.Anio,
                DuracionMinutos = p.DuracionMinutos,
                Rating = p.Rating,
                EnWatchList = idsEnWatchList.Contains(p.IdPelicula)
            }).ToList();

            var totalPaginas = total == 0
                ? 1
                : (int)Math.Ceiling(total / (double)tamanoPagina);

            return new CatalogoResultadoDto
            {
                Items = dtoItems,
                PaginaActual = pagina,
                TamanoPagina = tamanoPagina,
                TotalRegistros = total,
                TotalPaginas = totalPaginas
            };
        }

        public Task<List<Genero>> ObtenerGenerosAsync()
            => _peliculaRepositorio.ObtenerGenerosAsync();

        public Task<List<int>> ObtenerAniosDisponiblesAsync()
            => _peliculaRepositorio.ObtenerAniosDisponiblesAsync();
    }
}
