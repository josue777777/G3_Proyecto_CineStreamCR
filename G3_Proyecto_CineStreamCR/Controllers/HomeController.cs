using G3_Proyecto_CineStreamCR.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace G3_Proyecto_CineStreamCR.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var idUsuario =
                HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuario == null)
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            return View();
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