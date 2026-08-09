using Microsoft.AspNetCore.Mvc;

namespace G3_Proyecto_CineStreamCR.Controllers
{
    public class PeliculasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
