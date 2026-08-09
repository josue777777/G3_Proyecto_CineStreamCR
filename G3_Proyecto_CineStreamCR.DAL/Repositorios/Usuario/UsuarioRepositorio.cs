using G3_Proyecto_CineStreamCR.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace G3_Proyecto_CineStreamCR.DAL.Repositorios.Usuario
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtiene un usuario mediante correo o nombre de usuario.
        // Se utiliza AsNoTracking porque la consulta es únicamente de lectura.
        public async Task<Entidades.Usuario?> ObtenerPorIdentificadorAsync(
            string identificador)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u =>
                    u.Correo == identificador ||
                    u.NombreUsuario == identificador);
        }

        // Comprueba si existe un usuario con el correo indicado.
        public async Task<bool> ExisteCorreoAsync(string correo)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .AnyAsync(u => u.Correo == correo);
        }

        // Comprueba si existe un usuario con el nombre indicado.
        public async Task<bool> ExisteNombreUsuarioAsync(
            string nombreUsuario)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .AnyAsync(u => u.NombreUsuario == nombreUsuario);
        }
    }
}