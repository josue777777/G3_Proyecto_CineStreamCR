namespace G3_Proyecto_CineStreamCR.DAL.Entidades
{
    // Representa la calificación y reseña de un usuario para una película.
    public class Calificacion
    {
        public int IdCalificacion { get; set; }

        public int IdUsuario { get; set; }

        public int IdPelicula { get; set; }

        // La puntuación válida será de 1 a 10.
        public int Puntuacion { get; set; }

        // La reseña es opcional.
        public string? Resena { get; set; }

        // Propiedades de navegación.
        public Usuario Usuario { get; set; } = null!;

        public Pelicula Pelicula { get; set; } = null!;
    }
}