using System.Collections.Generic;
using System.Threading.Tasks;
using G3_Proyecto_CineStreamCR.BLL.Dtos;

namespace G3_Proyecto_CineStreamCR.BLL.Services.Pelicula;

public interface IPeliculaServicio
{
    Task<IEnumerable<PeliculaDto>> ObtenerTodasAsync();
    Task<PeliculaDto?> ObtenerPorIdAsync(int id);
}
