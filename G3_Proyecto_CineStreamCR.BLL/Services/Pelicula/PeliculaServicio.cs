using System.Collections.Generic;
using System.Threading.Tasks;
using G3_Proyecto_CineStreamCR.BLL.Dtos;

namespace G3_Proyecto_CineStreamCR.BLL.Services.Pelicula;

public class PeliculaServicio : IPeliculaServicio
{
    // Implementación en memoria de ejemplo. Reemplazar por llamada a DAL.
    private static readonly List<PeliculaDto> _peliculas = new()
    {
        new PeliculaDto { Id = 1, Titulo = "Pelicula A", Anio = 2023 },
        new PeliculaDto { Id = 2, Titulo = "Pelicula B", Anio = 2022 }
    };

    public Task<IEnumerable<PeliculaDto>> ObtenerTodasAsync()
    {
        return Task.FromResult<IEnumerable<PeliculaDto>>(_peliculas);
    }

    public Task<PeliculaDto?> ObtenerPorIdAsync(int id)
    {
        var p = _peliculas.Find(x => x.Id == id);
        return Task.FromResult<PeliculaDto?>(p);
    }
}
