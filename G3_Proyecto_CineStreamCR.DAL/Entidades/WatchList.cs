namespace G3_Proyecto_CineStreamCR.DAL.Entidades
{
    // Representa una lista personalizada de películas creada por un usuario.
    public class WatchList
    {
        public int IdWatchList { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        // Usuario propietario de la WatchList.
        public int IdUsuario { get; set; }

        public Usuario Usuario { get; set; } = null!;

        // Películas agregadas a esta WatchList.
        public ICollection<WatchListPelicula> WatchListPeliculas { get; set; }
            = new List<WatchListPelicula>();
    }
}