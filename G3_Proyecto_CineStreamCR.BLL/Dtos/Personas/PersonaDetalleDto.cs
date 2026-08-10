namespace G3_Proyecto_CineStreamCR.BLL.Dtos.Personas
{
    // Perfil público de una persona (director y/o actor) junto con
    // su filmografía dentro de la plataforma.
    public class PersonaDetalleDto
    {
        public int IdPersona { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Nacionalidad { get; set; }

        public string? Biografia { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string? FotoUrl { get; set; }

        public List<PersonaFilmografiaItemDto> PeliculasComoDirector { get; set; } = new();

        public List<PersonaFilmografiaItemDto> PeliculasComoActor { get; set; } = new();
    }

    // Una película dentro de la filmografía de una persona.
    public class PersonaFilmografiaItemDto
    {
        public int IdPelicula { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public int Anio { get; set; }

        public string? PosterUrl { get; set; }

        // Solo aplica cuando la película se lista dentro del elenco (como actor).
        public string? Personaje { get; set; }
    }
}
