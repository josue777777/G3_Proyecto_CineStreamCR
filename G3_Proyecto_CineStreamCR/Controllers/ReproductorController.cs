using G3_Proyecto_CineStreamCR.BLL.Services.Reproduccion;
using G3_Proyecto_CineStreamCR.DAL.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace G3_Proyecto_CineStreamCR.Controllers
{
    public class ReproductorController : Controller
    {
        private readonly IReproduccionService _reproduccionService;
        private readonly ApplicationDbContext _context;

        public ReproductorController(IReproduccionService reproduccionService, ApplicationDbContext context)
        {
            _reproduccionService = reproduccionService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Ver(int id)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula == null)
            {
                return NotFound();
            }

            var segundoActual = await _reproduccionService.ObtenerProgresoAsync(idUsuario.Value, id);
            ViewBag.SegundoActual = segundoActual;

            return View(pelicula);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarProgreso([FromBody] ProgresoRequest request)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null)
            {
                return Unauthorized();
            }

            if (request == null)
            {
                return BadRequest("La solicitud no contiene datos.");
            }

            var exito = await _reproduccionService.GuardarProgresoAsync(
                idUsuario.Value, 
                request.IdPelicula, 
                request.SegundoActual);

            return Json(new { success = exito });
        }
    }

    public class ProgresoRequest
    {
        public int IdPelicula { get; set; }
        public int SegundoActual { get; set; }
    }
}
