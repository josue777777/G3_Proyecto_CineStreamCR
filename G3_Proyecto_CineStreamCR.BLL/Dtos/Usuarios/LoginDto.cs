namespace G3_Proyecto_CineStreamCR.BLL.Dtos.Usuarios
{
    public class LoginDto
    {
        // Permite ingresar utilizando correo o nombre de usuario.
        public string Identificador { get; set; } = string.Empty;

        // Contraseña ingresada por el usuario.
        public string Contrasenna { get; set; } = string.Empty;
    }
}