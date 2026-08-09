namespace G3_Proyecto_CineStreamCR.DAL.Entidades
{
    // Representa un género cinematográfico.
    public class Genero
    {
        public int IdGenero { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public ICollection<PeliculaGenero> PeliculaGeneros { get; set; }
            = new List<PeliculaGenero>();
    }
}