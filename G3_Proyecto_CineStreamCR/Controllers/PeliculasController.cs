using G3_Proyecto_CineStreamCR.BLL.Dtos;
using G3_Proyecto_CineStreamCR.BLL.Services.Calificacion;
using G3_Proyecto_CineStreamCR.DAL.Data;
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
        private readonly ApplicationDbContext _context;

        public PeliculasController(ICalificacionService calificacionService, ApplicationDbContext context)
        {
            _calificacionService = calificacionService;
            _context = context;
        }

        // Catálogo de películas
        public async Task<IActionResult> Index()
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var peliculas = await _context.Peliculas
                .AsNoTracking()
                .Include(p => p.Director)
                .ToListAsync();

            return View(peliculas);
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

            return View(pelicula);
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
}
