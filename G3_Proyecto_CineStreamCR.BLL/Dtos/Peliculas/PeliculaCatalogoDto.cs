namespace G3_Proyecto_CineStreamCR.BLL.Dtos.Peliculas
{
    // Datos que necesita cada tarjeta del catálogo.
    public class PeliculaCatalogoDto
    {
        public int IdPelicula { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string? Sinopsis { get; set; }

        public string? PosterUrl { get; set; }

        public int Anio { get; set; }

        public int DuracionMinutos { get; set; }

        public double Rating { get; set; }

        // Indica si la película ya está en la WatchList del usuario actual.
        public bool EnWatchList { get; set; }
    }
}
