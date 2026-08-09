namespace G3_Proyecto_CineStreamCR.DAL.Entidades
{
    // Relaciona una película con un actor y el personaje interpretado.
    public class PeliculaActor
    {
        public int IdPelicula { get; set; }

        // IdPersona de la persona que participa como actor.
        public int IdActor { get; set; }

        public string Personaje { get; set; } = string.Empty;

        // Propiedades de navegación.
        public Pelicula Pelicula { get; set; } = null!;

        public Persona Actor { get; set; } = null!;
    }
}