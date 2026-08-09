namespace G3_Proyecto_CineStreamCR.DAL.Entidades
{
    // Relación muchos a muchos entre películas y géneros.
    public class PeliculaGenero
    {
        public int IdPelicula { get; set; }

        public int IdGenero { get; set; }

        // Propiedades de navegación.
        public Pelicula Pelicula { get; set; } = null!;

        public Genero Genero { get; set; } = null!;
    }
}