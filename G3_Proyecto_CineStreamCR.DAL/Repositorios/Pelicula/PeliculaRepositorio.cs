using System.Collections.Generic;
using System.Threading.Tasks;
using G3_Proyecto_CineStreamCR.DAL.Entidades;

namespace G3_Proyecto_CineStreamCR.DAL.Repositorios;

public class PeliculaRepositorio : IPeliculaRepositorio
{
    // Implementación en memoria de ejemplo. Reemplazar por EF Core.
    private static readonly List<Pelicula> _peliculas = new()
    {
        new Pelicula { Id = 1, Titulo = "Pelicula A", Anio = 2023 },
        new Pelicula { Id = 2, Titulo = "Pelicula B", Anio = 2022 }
    };

    public Task<IEnumerable<Pelicula>> ObtenerTodasAsync()
    {
        return Task.FromResult<IEnumerable<Pelicula>>(_peliculas);
    }

    public Task<Pelicula?> ObtenerPorIdAsync(int id)
    {
        var p = _peliculas.Find(x => x.Id == id);
        return Task.FromResult<Pelicula?>(p);
    }
}
