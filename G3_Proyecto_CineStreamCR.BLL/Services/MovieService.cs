using System.Collections.Generic;
using System.Threading.Tasks;

namespace G3_Proyecto_CineStreamCR.BLL.Services;

public class MovieDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
}

public interface IMovieService
{
    Task<IEnumerable<MovieDto>> GetAllAsync();
    Task<MovieDto?> GetByIdAsync(int id);
}

public class MovieService : IMovieService
{
    // Ejemplo simple en memoria. Reemplazar por acceso a datos real.
    private static readonly List<MovieDto> _movies = new()
    {
        new MovieDto { Id = 1, Title = "El Gran Cine", Year = 2023 },
        new MovieDto { Id = 2, Title = "Noches de Película", Year = 2022 }
    };

    public Task<IEnumerable<MovieDto>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<MovieDto>>(_movies);
    }

    public Task<MovieDto?> GetByIdAsync(int id)
    {
        var m = _movies.Find(x => x.Id == id);
        return Task.FromResult<MovieDto?>(m);
    }
}
