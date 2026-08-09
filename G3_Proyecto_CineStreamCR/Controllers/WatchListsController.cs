using Microsoft.AspNetCore.Mvc;

namespace G3_Proyecto_CineStreamCR.Controllers
{
    public class WatchListsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
