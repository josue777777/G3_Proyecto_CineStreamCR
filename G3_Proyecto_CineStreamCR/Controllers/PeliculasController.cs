using G3_Proyecto_CineStreamCR.BLL.Dtos;
using G3_Proyecto_CineStreamCR.BLL.Dtos.Peliculas;
using G3_Proyecto_CineStreamCR.BLL.Services.Calificacion;
using G3_Proyecto_CineStreamCR.BLL.Services.Pelicula;
using G3_Proyecto_CineStreamCR.BLL.Services.WatchList;
using G3_Proyecto_CineStreamCR.DAL.Data;
using G3_Proyecto_CineStreamCR.Models.Peliculas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace G3_Proyecto_CineStreamCR.Controllers
{
    public class PeliculasController : Controller
    {
        private readonly ICalificacionService _calificacionService;
        private readonly IPeliculaService _peliculaService;
        private readonly IWatchListService _watchListService;
        private readonly ApplicationDbContext _context;

        public PeliculasController(
            ICalificacionService calificacionService,
            IPeliculaService peliculaService,
            IWatchListService watchListService,
            ApplicationDbContext context)
        {
            _calificacionService = calificacionService;
            _peliculaService = peliculaService;
            _watchListService = watchListService;
            _context = context;
        }

        // Catálogo de películas: búsqueda, filtros, orden y paginación.
        public async Task<IActionResult> Index(CatalogoFiltroDto filtro)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var modelo = await ConstruirCatalogoViewModelAsync(filtro, idUsuario);
            return View(modelo);
        }

        // Igual que Index, pero devuelve únicamente la porción de la grilla
        // (tarjetas + paginación) para las actualizaciones AJAX del catálogo.
        [HttpGet]
        public async Task<IActionResult> Buscar(CatalogoFiltroDto filtro)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            var modelo = await ConstruirCatalogoViewModelAsync(filtro, idUsuario);
            return PartialView("_CatalogoGrid", modelo);
        }

        private async Task<CatalogoIndexViewModel> ConstruirCatalogoViewModelAsync(CatalogoFiltroDto filtro, int? idUsuario)
        {
            filtro ??= new CatalogoFiltroDto();

            var resultado = await _peliculaService.ObtenerCatalogoAsync(filtro, idUsuario);
            var generos = await _peliculaService.ObtenerGenerosAsync();
            var anios = await _peliculaService.ObtenerAniosDisponiblesAsync();

            return new CatalogoIndexViewModel
            {
                Resultado = resultado,
                Filtro = filtro,
                Generos = generos,
                Anios = anios
            };
        }

        // Detalles de una película
        public async Task<IActionResult> Detalle(int id)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var pelicula = await _context.Peliculas
                .AsNoTracking()
                .Include(p => p.Director)
                .Include(p => p.PeliculaGeneros).ThenInclude(pg => pg.Genero)
                .Include(p => p.PeliculaActores).ThenInclude(pa => pa.Actor)
                .FirstOrDefaultAsync(p => p.IdPelicula == id);

            if (pelicula == null)
            {
                return NotFound();
            }

            ViewBag.EnWatchList = await _watchListService.EstaEnWatchListAsync(idUsuario.Value, id);

            return View(pelicula);
        }

        // Agrega o quita una película de la WatchList del usuario actual (AJAX POST).
        [HttpPost]
        public async Task<IActionResult> ToggleWatchList([FromBody] ToggleWatchListRequest request)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null)
            {
                return Unauthorized(new { success = false, message = "Debes iniciar sesión para usar tu WatchList." });
            }

            if (request == null || request.IdPelicula <= 0)
            {
                return BadRequest(new { success = false, message = "Película inválida." });
            }

            var resultado = await _watchListService.ToggleAsync(idUsuario.Value, request.IdPelicula);

            if (!resultado.Exitoso)
            {
                return BadRequest(new { success = false, message = resultado.Mensaje });
            }

            return Json(new { success = true, enWatchList = resultado.EnWatchList, message = resultado.Mensaje });
        }

        // Agregar una reseña (AJAX POST)
        [HttpPost]
        public async Task<IActionResult> AgregarResena([FromBody] CalificacionDto dto)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null)
            {
                return Unauthorized();
            }

            if (dto == null)
            {
                return BadRequest(new { success = false, message = "Datos inválidos" });
            }

            dto.IdUsuario = idUsuario.Value;

            var resultado = await _calificacionService.AgregarOActualizarAsync(dto);
            if (resultado.Exitoso)
            {
                return Json(new { success = true, message = resultado.Mensaje });
            }
            else
            {
                return BadRequest(new { success = false, message = resultado.Mensaje });
            }
        }

        // Obtener reseñas por película (AJAX GET)
        [HttpGet]
        public async Task<IActionResult> ObtenerResenas(int id)
        {
            var resenas = await _calificacionService.ObtenerPorPeliculaAsync(id);
            return Json(resenas);
        }
    }

    public class ToggleWatchListRequest
    {
        public int IdPelicula { get; set; }
    }
}
