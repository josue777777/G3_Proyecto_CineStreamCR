using System.ComponentModel.DataAnnotations;

namespace G3_Proyecto_CineStreamCR.Models.Auth
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "Debe ingresar un nombre de usuario.")]
        [StringLength(
            50,
            MinimumLength = 3,
            ErrorMessage = "El nombre de usuario debe tener entre 3 y 50 caracteres.")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe ingresar un correo.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo válido.")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe ingresar una contraseña.")]
        [StringLength(
            100,
            MinimumLength = 6,
            ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [DataType(DataType.Password)]
        public string Contrasenna { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe confirmar la contraseña.")]
        [DataType(DataType.Password)]
        [Compare(
            nameof(Contrasenna),
            ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarContrasenna { get; set; } = string.Empty;
    }
}