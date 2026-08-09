namespace G3_Proyecto_CineStreamCR.DAL.Entidades
{
    // Representa una película disponible en la plataforma.
    public class Pelicula
    {
        public int IdPelicula { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string? Sinopsis { get; set; }

        public int Anio { get; set; }

        public int DuracionMinutos { get; set; }

        public string? PosterUrl { get; set; }

        public string? VideoUrl { get; set; }

        public double Rating { get; set; }

        // Director de la película.
        public int IdDirector { get; set; }

        public Persona? Director { get; set; }

        // Relaciones.
        public ICollection<PeliculaGenero> PeliculaGeneros { get; set; }
            = new List<PeliculaGenero>();

        public ICollection<PeliculaActor> PeliculaActores { get; set; }
            = new List<PeliculaActor>();

        public ICollection<WatchListPelicula> WatchListPeliculas { get; set; }
            = new List<WatchListPelicula>();

        public ICollection<Calificacion> Calificaciones { get; set; }
            = new List<Calificacion>();

        public ICollection<Reproduccion> Reproducciones { get; set; }
            = new List<Reproduccion>();
    }
}