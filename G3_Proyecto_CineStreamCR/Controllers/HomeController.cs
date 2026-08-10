using G3_Proyecto_CineStreamCR.Models;
using G3_Proyecto_CineStreamCR.BLL.Dtos.Peliculas;
using G3_Proyecto_CineStreamCR.BLL.Services.Pelicula;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace G3_Proyecto_CineStreamCR.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPeliculaService _peliculaService;

        public HomeController(IPeliculaService peliculaService)
        {
            _peliculaService = peliculaService;
        }

        public async Task<IActionResult> Index()
        {
            var idUsuario =
                HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuario == null)
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            var filtro = new CatalogoFiltroDto
            {
                OrdenarPor = "anio",
                Direccion = "desc",
                Pagina = 1,
                TamanoPagina = 6
            };

            var peliculas = await _peliculaService.ObtenerCatalogoAsync(
                filtro,
                idUsuario.Value);

            return View(peliculas);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId =
                    Activity.Current?.Id ??
                    HttpContext.TraceIdentifier
            });
        }
    }
}
