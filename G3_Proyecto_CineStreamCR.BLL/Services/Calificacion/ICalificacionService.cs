using G3_Proyecto_CineStreamCR.BLL.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G3_Proyecto_CineStreamCR.BLL.Services.Calificacion
{
    public interface ICalificacionService
    {
        Task<IEnumerable<CalificacionDto>> ObtenerPorPeliculaAsync(int idPelicula);
        Task<(bool Exitoso, string Mensaje)> AgregarOActualizarAsync(CalificacionDto dto);
        Task<double> ObtenerPromedioPeliculaAsync(int idPelicula);
    }
}
