using G3_Proyecto_CineStreamCR.DAL.Entidades;

namespace G3_Proyecto_CineStreamCR.DAL.Repositorios.Usuario
{
    public interface IUsuarioRepositorio
    {
        // Busca un usuario por correo o nombre de usuario.
        Task<Entidades.Usuario?> ObtenerPorIdentificadorAsync(
            string identificador);

        // Verifica si ya existe un correo.
        Task<bool> ExisteCorreoAsync(string correo);

        // Verifica si ya existe un nombre de usuario.
        Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario);

        // Crea un nuevo usuario.
        Task<bool> CrearUsuarioAsync(Entidades.Usuario usuario);
    }
}