namespace G3_Proyecto_CineStreamCR.DAL.Entidades
{
    // Relaciona una WatchList con las películas que contiene.
    public class WatchListPelicula
    {
        public int IdWatchList { get; set; }

        public int IdPelicula { get; set; }

        // Propiedades de navegación.
        public WatchList WatchList { get; set; } = null!;

        public Pelicula Pelicula { get; set; } = null!;
    }
}