using G3_Proyecto_CineStreamCR.BLL.Services.Persona;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace G3_Proyecto_CineStreamCR.Controllers
{
    public class PersonasController : Controller
    {
        private readonly IPersonaService _personaService;

        public PersonasController(IPersonaService personaService)
        {
            _personaService = personaService;
        }

        public async Task<IActionResult> Index()
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var personas = await _personaService.ObtenerTodasAsync();
            return View(personas);
        }

        // Perfil público de una persona (director o actor), enlazado
        // desde el detalle de película.
        public async Task<IActionResult> Detalle(int id)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var persona = await _personaService.ObtenerDetalleAsync(id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }
    }
}
