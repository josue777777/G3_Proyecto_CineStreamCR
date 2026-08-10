using G3_Proyecto_CineStreamCR.BLL.Dtos;
using G3_Proyecto_CineStreamCR.DAL.Data;
using G3_Proyecto_CineStreamCR.DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G3_Proyecto_CineStreamCR.BLL.Services.Calificacion
{
    public class CalificacionService : ICalificacionService
    {
        private readonly ApplicationDbContext _context;

        public CalificacionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CalificacionDto>> ObtenerPorPeliculaAsync(int idPelicula)
        {
            return await _context.Calificaciones
                .AsNoTracking()
                .Where(c => c.IdPelicula == idPelicula)
                .Include(c => c.Usuario)
                .Select(c => new CalificacionDto
                {
                    IdCalificacion = c.IdCalificacion,
                    Puntuacion = c.Puntuacion,
                    Resena = c.Resena,
                    IdPelicula = c.IdPelicula,
                    IdUsuario = c.IdUsuario,
                    NombreUsuario = c.Usuario.NombreUsuario
                })
                .ToListAsync();
        }

        public async Task<(bool Exitoso, string Mensaje)> AgregarOActualizarAsync(CalificacionDto dto)
        {
            if (dto.Puntuacion < 1 || dto.Puntuacion > 10)
                return (false, "La calificación debe estar entre 1 y 10.");

            var calificacion = await _context.Calificaciones
                .FirstOrDefaultAsync(c => c.IdUsuario == dto.IdUsuario && c.IdPelicula == dto.IdPelicula);

            if (calificacion == null)
            {
                calificacion = new DAL.Entidades.Calificacion
                {
                    IdUsuario = dto.IdUsuario,
                    IdPelicula = dto.IdPelicula,
                    Puntuacion = dto.Puntuacion,
                    Resena = dto.Resena
                };
                await _context.Calificaciones.AddAsync(calificacion);
            }
            else
            {
                calificacion.Puntuacion = dto.Puntuacion;
                calificacion.Resena = dto.Resena;
                _context.Calificaciones.Update(calificacion);
            }

            var guardado = await _context.SaveChangesAsync() > 0;
            
            // Recalcular rating de la película
            if (guardado)
            {
                await ActualizarRatingPeliculaAsync(dto.IdPelicula);
            }

            return guardado 
                ? (true, "Reseña guardada exitosamente.") 
                : (false, "No se pudo guardar la reseña.");
        }

        public async Task<double> ObtenerPromedioPeliculaAsync(int idPelicula)
        {
            var calificaciones = await _context.Calificaciones
                .AsNoTracking()
                .Where(c => c.IdPelicula == idPelicula)
                .ToListAsync();

            if (!calificaciones.Any()) return 0;
            return Math.Round(calificaciones.Average(c => c.Puntuacion), 1);
        }

        private async Task ActualizarRatingPeliculaAsync(int idPelicula)
        {
            var pelicula = await _context.Peliculas.FindAsync(idPelicula);
            if (pelicula != null)
            {
                pelicula.Rating = await ObtenerPromedioPeliculaAsync(idPelicula);
                _context.Peliculas.Update(pelicula);
                await _context.SaveChangesAsync();
            }
        }
    }
}
