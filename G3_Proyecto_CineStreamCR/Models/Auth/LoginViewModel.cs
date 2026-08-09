using System.ComponentModel.DataAnnotations;

namespace G3_Proyecto_CineStreamCR.Models.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Debe ingresar su correo o nombre de usuario.")]
        public string Identificador { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe ingresar la contraseña.")]
        [DataType(DataType.Password)]
        public string Contrasenna { get; set; } = string.Empty;
    }
}