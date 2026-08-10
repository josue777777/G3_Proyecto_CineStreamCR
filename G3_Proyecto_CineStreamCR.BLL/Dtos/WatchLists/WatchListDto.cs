using System.ComponentModel.DataAnnotations;

namespace G3_Proyecto_CineStreamCR.BLL.Dtos.WatchLists
{
    public class WatchListResumenDto
    {
        public int IdWatchList { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public int CantidadPeliculas { get; set; }
    }

    public class WatchListDetalleDto
    {
        public int IdWatchList { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public List<WatchListPeliculaDto> Peliculas { get; set; } = new();
    }

    public class WatchListPeliculaDto
    {
        public int IdPelicula { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string? PosterUrl { get; set; }

        public int Anio { get; set; }

        public int DuracionMinutos { get; set; }

        public double Rating { get; set; }
    }

    public class WatchListFormDto
    {
        public int IdWatchList { get; set; }

        [Required(ErrorMessage = "El nombre de la lista es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        public string? Descripcion { get; set; }
    }

    public class WatchListSeleccionDto
    {
        public int IdPelicula { get; set; }

        public string TituloPelicula { get; set; } = string.Empty;

        public List<WatchListSeleccionItemDto> Listas { get; set; } = new();
    }

    public class WatchListSeleccionItemDto
    {
        public int IdWatchList { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public bool Seleccionada { get; set; }
    }

    public class WatchListOperacionResultDto
    {
        public bool Exitoso { get; set; }

        public string Mensaje { get; set; } = string.Empty;
    }
}
