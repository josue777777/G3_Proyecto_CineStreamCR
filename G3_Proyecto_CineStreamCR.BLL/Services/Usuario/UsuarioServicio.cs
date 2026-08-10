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

        public async Task<(bool Exitoso, string Mensaje)>
            RegistrarAsync(RegistroDto registroDto)
        {
            if (string.IsNullOrWhiteSpace(registroDto.NombreUsuario) ||
                string.IsNullOrWhiteSpace(registroDto.Correo) ||
                string.IsNullOrWhiteSpace(registroDto.Contrasenna))
            {
                return (
                    false,
                    "Debe completar todos los campos."
                );
            }

            var nombreUsuario =
                registroDto.NombreUsuario.Trim();

            var correo =
                registroDto.Correo.Trim();

            // Verifica que el nombre de usuario no esté ocupado.
            var existeNombreUsuario =
                await _usuarioRepositorio
                    .ExisteNombreUsuarioAsync(nombreUsuario);

            if (existeNombreUsuario)
            {
                return (
                    false,
                    "El nombre de usuario ya está registrado."
                );
            }

            // Verifica que el correo no esté ocupado.
            var existeCorreo =
                await _usuarioRepositorio
                    .ExisteCorreoAsync(correo);

            if (existeCorreo)
            {
                return (
                    false,
                    "El correo ya está registrado."
                );
            }

            var usuario = new EntidadUsuario
            {
                NombreUsuario = nombreUsuario,
                Correo = correo
            };

            // Genera el hash seguro de la contraseña.
            usuario.PasswordHash =
                _passwordHasher.HashPassword(
                    usuario,
                    registroDto.Contrasenna);

            var creado =
                await _usuarioRepositorio
                    .CrearUsuarioAsync(usuario);

            if (!creado)
            {
                return (
                    false,
                    "No se pudo crear el usuario."
                );
            }

            return (
                true,
                "Usuario registrado correctamente."
            );
        }
    }
}