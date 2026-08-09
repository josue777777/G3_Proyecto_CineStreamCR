namespace G3_Proyecto_CineStreamCR.DAL.Entidades
{
    // Representa un usuario registrado en CineStream CR.
    public class Usuario
    {
        public int IdUsuario { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        // Nunca se almacena la contraseña en texto plano.
        public string PasswordHash { get; set; } = string.Empty;

        // Relaciones.
        public ICollection<WatchList> WatchLists { get; set; }
            = new List<WatchList>();

        public ICollection<Calificacion> Calificaciones { get; set; }
            = new List<Calificacion>();

        public ICollection<Reproduccion> Reproducciones { get; set; }
            = new List<Reproduccion>();
    }
}