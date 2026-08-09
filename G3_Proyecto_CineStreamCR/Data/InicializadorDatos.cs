using G3_Proyecto_CineStreamCR.DAL.Data;
using G3_Proyecto_CineStreamCR.DAL.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace G3_Proyecto_CineStreamCR.Data
{
    public static class InicializadorDatos
    {
        public static async Task InicializarAsync(
            ApplicationDbContext context)
        {
            // Solo crea el usuario inicial si todavía no existe.
            var existeUsuario = await context.Usuarios
                .AsNoTracking()
                .AnyAsync(u => u.NombreUsuario == "admin");

            if (existeUsuario)
                return;

            var usuario = new Usuario
            {
                NombreUsuario = "admin",
                Correo = "admin@cinestreamcr.com"
            };

            var passwordHasher =
                new PasswordHasher<Usuario>();

            usuario.PasswordHash =
                passwordHasher.HashPassword(
                    usuario,
                    "Admin123*");

            await context.Usuarios.AddAsync(usuario);

            await context.SaveChangesAsync();
        }
    }
}