using G3_Proyecto_CineStreamCR.DAL.Data;
using G3_Proyecto_CineStreamCR.DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace G3_Proyecto_CineStreamCR.BLL.Services.Reproduccion
{
    public class ReproduccionService : IReproduccionService
    {
        private readonly ApplicationDbContext _context;

        public ReproduccionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> ObtenerProgresoAsync(int idUsuario, int idPelicula)
        {
            var repro = await _context.Reproducciones
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.IdUsuario == idUsuario && r.IdPelicula == idPelicula);
            return repro?.SegundoActual ?? 0;
        }

        public async Task<bool> GuardarProgresoAsync(int idUsuario, int idPelicula, int segundoActual)
        {
            var repro = await _context.Reproducciones
                .FirstOrDefaultAsync(r => r.IdUsuario == idUsuario && r.IdPelicula == idPelicula);

            if (repro == null)
            {
                repro = new DAL.Entidades.Reproduccion
                {
                    IdUsuario = idUsuario,
                    IdPelicula = idPelicula,
                    SegundoActual = segundoActual,
                    FechaUltimaReproduccion = DateTime.Now
                };
                await _context.Reproducciones.AddAsync(repro);
            }
            else
            {
                repro.SegundoActual = segundoActual;
                repro.FechaUltimaReproduccion = DateTime.Now;
                _context.Reproducciones.Update(repro);
            }

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
