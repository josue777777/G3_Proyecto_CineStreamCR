namespace G3_Proyecto_CineStreamCR.Models.Auth
{
    public class AuthViewModel
    {
        public LoginViewModel Login { get; set; }
            = new LoginViewModel();

        public RegistroViewModel Registro { get; set; }
            = new RegistroViewModel();

        // Indica qué formulario debe mostrarse cuando
        // la vista vuelve con errores.
        public bool MostrarRegistro { get; set; }
    }
}