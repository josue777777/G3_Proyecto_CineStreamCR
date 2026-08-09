namespace G3_Proyecto_CineStreamCR.DAL.Entidades
{
    // Guarda el progreso de reproducción de una película para un usuario.
    public class Reproduccion
    {
        public int IdReproduccion { get; set; }

        public int IdUsuario { get; set; }

        public int IdPelicula { get; set; }

        // Segundo en el que el usuario dejó la reproducción.
        public int SegundoActual { get; set; }

        public DateTime? FechaUltimaReproduccion { get; set; }

        // Propiedades de navegación.
        public Usuario Usuario { get; set; } = null!;

        public Pelicula Pelicula { get; set; } = null!;
    }
}