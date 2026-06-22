// El DbContext depende de Microsoft.EntityFrameworkCore. Si aún no desea
// añadir el paquete, deje la clase como placeholder y añada el paquete
// Microsoft.EntityFrameworkCore más adelante.
using Microsoft.EntityFrameworkCore;

namespace G3_Proyecto_CineStreamCR.DAL.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // DbSets de las entidades
    // public DbSet<Pelicula> Peliculas { get; set; }
}
