namespace G3_Proyecto_CineStreamCR.DAL.Entidades
{
    // Representa una persona que puede participar como director o actor.
    public class Persona
    {
        public int IdPersona { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Nacionalidad { get; set; }

        public string? Biografia { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string? FotoUrl { get; set; }

        // Películas dirigidas por esta persona.
        public ICollection<Pelicula> PeliculasDirigidas { get; set; }
            = new List<Pelicula>();

        // Participaciones como actor.
        public ICollection<PeliculaActor> PeliculasComoActor { get; set; }
            = new List<PeliculaActor>();
    }
}