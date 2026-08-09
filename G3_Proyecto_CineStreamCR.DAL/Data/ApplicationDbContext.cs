using G3_Proyecto_CineStreamCR.DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace G3_Proyecto_CineStreamCR.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // =====================================================
        // DbSets
        // =====================================================

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Persona> Personas { get; set; }

        public DbSet<Genero> Generos { get; set; }

        public DbSet<Pelicula> Peliculas { get; set; }

        public DbSet<PeliculaGenero> PeliculaGeneros { get; set; }

        public DbSet<PeliculaActor> PeliculaActores { get; set; }

        public DbSet<WatchList> WatchLists { get; set; }

        public DbSet<WatchListPelicula> WatchListPeliculas { get; set; }

        public DbSet<Calificacion> Calificaciones { get; set; }

        public DbSet<Reproduccion> Reproducciones { get; set; }


        // =====================================================
        // Configuración Fluent API
        // =====================================================

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // =================================================
            // USUARIO
            // =================================================

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.IdUsuario);

                entity.Property(u => u.NombreUsuario)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(u => u.Correo)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(u => u.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasIndex(u => u.NombreUsuario)
                    .IsUnique();

                entity.HasIndex(u => u.Correo)
                    .IsUnique();
            });


            // =================================================
            // PERSONA
            // =================================================

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(p => p.IdPersona);

                entity.Property(p => p.Nombre)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(p => p.Nacionalidad)
                    .HasMaxLength(100);

                entity.Property(p => p.Biografia)
                    .HasMaxLength(2000);

                entity.Property(p => p.FotoUrl)
                    .HasMaxLength(500);
            });


            // =================================================
            // GENERO
            // =================================================

            modelBuilder.Entity<Genero>(entity =>
            {
                entity.HasKey(g => g.IdGenero);

                entity.Property(g => g.Nombre)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.HasIndex(g => g.Nombre)
                    .IsUnique();
            });


            // =================================================
            // PELICULA
            // =================================================

            modelBuilder.Entity<Pelicula>(entity =>
            {
                entity.HasKey(p => p.IdPelicula);

                entity.Property(p => p.Titulo)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(p => p.Sinopsis)
                    .HasMaxLength(3000);

                entity.Property(p => p.PosterUrl)
                    .HasMaxLength(500);

                entity.Property(p => p.VideoUrl)
                    .HasMaxLength(500);

                entity.Property(p => p.Anio)
                    .IsRequired();

                entity.Property(p => p.DuracionMinutos)
                    .IsRequired();

                entity.Property(p => p.Rating)
                    .IsRequired();

                // Relación:
                // Una Persona puede dirigir muchas películas.
                entity.HasOne(p => p.Director)
                    .WithMany(p => p.PeliculasDirigidas)
                    .HasForeignKey(p => p.IdDirector)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =================================================
            // PELICULA - GENERO
            // =================================================

            modelBuilder.Entity<PeliculaGenero>(entity =>
            {
                entity.HasKey(pg => new
                {
                    pg.IdPelicula,
                    pg.IdGenero
                });

                entity.HasOne(pg => pg.Pelicula)
                    .WithMany(p => p.PeliculaGeneros)
                    .HasForeignKey(pg => pg.IdPelicula)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pg => pg.Genero)
                    .WithMany(g => g.PeliculaGeneros)
                    .HasForeignKey(pg => pg.IdGenero)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            // =================================================
            // PELICULA - ACTOR
            // =================================================

            modelBuilder.Entity<PeliculaActor>(entity =>
            {
                entity.HasKey(pa => new
                {
                    pa.IdPelicula,
                    pa.IdActor
                });

                entity.Property(pa => pa.Personaje)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(pa => pa.Pelicula)
                    .WithMany(p => p.PeliculaActores)
                    .HasForeignKey(pa => pa.IdPelicula)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pa => pa.Actor)
                    .WithMany(p => p.PeliculasComoActor)
                    .HasForeignKey(pa => pa.IdActor)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =================================================
            // WATCHLIST
            // =================================================

            modelBuilder.Entity<WatchList>(entity =>
            {
                entity.HasKey(w => w.IdWatchList);

                entity.Property(w => w.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(w => w.Descripcion)
                    .HasMaxLength(500);

                entity.HasOne(w => w.Usuario)
                    .WithMany(u => u.WatchLists)
                    .HasForeignKey(w => w.IdUsuario)
                    .OnDelete(DeleteBehavior.Cascade);

                // Un mismo usuario no puede tener dos WatchLists
                // con exactamente el mismo nombre.
                entity.HasIndex(w => new
                {
                    w.IdUsuario,
                    w.Nombre
                })
                .IsUnique();
            });


            // =================================================
            // WATCHLIST - PELICULA
            // =================================================

            modelBuilder.Entity<WatchListPelicula>(entity =>
            {
                entity.HasKey(wp => new
                {
                    wp.IdWatchList,
                    wp.IdPelicula
                });

                entity.HasOne(wp => wp.WatchList)
                    .WithMany(w => w.WatchListPeliculas)
                    .HasForeignKey(wp => wp.IdWatchList)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(wp => wp.Pelicula)
                    .WithMany(p => p.WatchListPeliculas)
                    .HasForeignKey(wp => wp.IdPelicula)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            // =================================================
            // CALIFICACION
            // =================================================

            modelBuilder.Entity<Calificacion>(entity =>
            {
                entity.HasKey(c => c.IdCalificacion);

                entity.Property(c => c.Puntuacion)
                    .IsRequired();

                entity.Property(c => c.Resena)
                    .HasMaxLength(2000);

                entity.HasOne(c => c.Usuario)
                    .WithMany(u => u.Calificaciones)
                    .HasForeignKey(c => c.IdUsuario)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.Pelicula)
                    .WithMany(p => p.Calificaciones)
                    .HasForeignKey(c => c.IdPelicula)
                    .OnDelete(DeleteBehavior.Cascade);

                // Un usuario mantiene una sola calificación
                // por película.
                entity.HasIndex(c => new
                {
                    c.IdUsuario,
                    c.IdPelicula
                })
                .IsUnique();
            });


            // =================================================
            // REPRODUCCION
            // =================================================

            modelBuilder.Entity<Reproduccion>(entity =>
            {
                entity.HasKey(r => r.IdReproduccion);

                entity.Property(r => r.SegundoActual)
                    .IsRequired();

                entity.HasOne(r => r.Usuario)
                    .WithMany(u => u.Reproducciones)
                    .HasForeignKey(r => r.IdUsuario)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Pelicula)
                    .WithMany(p => p.Reproducciones)
                    .HasForeignKey(r => r.IdPelicula)
                    .OnDelete(DeleteBehavior.Cascade);

                // Solo debe existir un progreso actual
                // por usuario y película.
                entity.HasIndex(r => new
                {
                    r.IdUsuario,
                    r.IdPelicula
                })
                .IsUnique();
            });
        }
    }
}