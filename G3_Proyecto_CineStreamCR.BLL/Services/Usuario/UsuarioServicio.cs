using G3_Proyecto_CineStreamCR.BLL.Dtos.Usuarios;
using G3_Proyecto_CineStreamCR.DAL.Repositorios.Usuario;
using Microsoft.AspNetCore.Identity;
using EntidadUsuario = G3_Proyecto_CineStreamCR.DAL.Entidades.Usuario;

namespace G3_Proyecto_CineStreamCR.BLL.Services.Usuario
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly PasswordHasher<EntidadUsuario> _passwordHasher;

        public UsuarioServicio(
            IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _passwordHasher = new PasswordHasher<EntidadUsuario>();
        }

        public async Task<(
            bool Exitoso,
            string Mensaje,
            EntidadUsuario? Usuario)>
            LoginAsync(LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Identificador) ||
                string.IsNullOrWhiteSpace(loginDto.Contrasenna))
            {
                return (
                    false,
                    "Debe ingresar usuario o correo y contraseña.",
                    null);
            }

            var usuario =
                await _usuarioRepositorio.ObtenerPorIdentificadorAsync(
                    loginDto.Identificador.Trim());

            if (usuario == null)
            {
                return (
                    false,
                    "El usuario no existe.",
                    null);
            }

            var resultado =
                _passwordHasher.VerifyHashedPassword(
                    usuario,
                    usuario.PasswordHash,
                    loginDto.Contrasenna);

            if (resultado == PasswordVerificationResult.Failed)
            {
                return (
                    false,
                    "La contraseña es incorrecta.",
                    null);
            }

            return (
                true,
                "Inicio de sesión correcto.",
                usuario);
        }
    }
}