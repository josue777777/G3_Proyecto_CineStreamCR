using System.Collections.Generic;
using System.Threading.Tasks;
using G3_Proyecto_CineStreamCR.DAL.Entidades;

namespace G3_Proyecto_CineStreamCR.DAL.Repositorios;

public interface IPeliculaRepositorio
{
    Task<IEnumerable<Pelicula>> ObtenerTodasAsync();
    Task<Pelicula?> ObtenerPorIdAsync(int id);
}
