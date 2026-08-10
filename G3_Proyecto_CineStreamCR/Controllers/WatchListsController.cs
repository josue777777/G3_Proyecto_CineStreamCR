using G3_Proyecto_CineStreamCR.BLL.Dtos.WatchLists;
using G3_Proyecto_CineStreamCR.BLL.Services.WatchList;
using Microsoft.AspNetCore.Mvc;

namespace G3_Proyecto_CineStreamCR.Controllers
{
    public class WatchListsController : Controller
    {
        private readonly IWatchListService _watchListService;

        public WatchListsController(IWatchListService watchListService)
        {
            _watchListService = watchListService;
        }

        public async Task<IActionResult> Index()
        {
            var idUsuario = ObtenerIdUsuario();
            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var listas = await _watchListService.ObtenerListasAsync(idUsuario.Value);
            return View(listas);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var idUsuario = ObtenerIdUsuario();
            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var lista = await _watchListService.ObtenerDetalleAsync(idUsuario.Value, id);
            if (lista == null)
            {
                return NotFound();
            }

            return View(lista);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            if (ObtenerIdUsuario() == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(new WatchListFormDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(WatchListFormDto model)
        {
            var idUsuario = ObtenerIdUsuario();
            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var resultado = await _watchListService.CrearAsync(idUsuario.Value, model);
            if (!resultado.Exitoso)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                return View(model);
            }

            TempData["Mensaje"] = resultado.Mensaje;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var idUsuario = ObtenerIdUsuario();
            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = await _watchListService.ObtenerFormularioAsync(idUsuario.Value, id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, WatchListFormDto model)
        {
            var idUsuario = ObtenerIdUsuario();
            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            model.IdWatchList = id;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var resultado = await _watchListService.EditarAsync(idUsuario.Value, model);
            if (!resultado.Exitoso)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                return View(model);
            }

            TempData["Mensaje"] = resultado.Mensaje;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var idUsuario = ObtenerIdUsuario();
            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var lista = await _watchListService.ObtenerDetalleAsync(idUsuario.Value, id);
            if (lista == null)
            {
                return NotFound();
            }

            return View(lista);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var idUsuario = ObtenerIdUsuario();
            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var resultado = await _watchListService.EliminarAsync(idUsuario.Value, id);
            TempData["Mensaje"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuitarPelicula(int idWatchList, int idPelicula)
        {
            var idUsuario = ObtenerIdUsuario();
            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var resultado = await _watchListService.QuitarPeliculaAsync(
                idUsuario.Value,
                idWatchList,
                idPelicula);

            TempData["Mensaje"] = resultado.Mensaje;
            return RedirectToAction(nameof(Detalle), new { id = idWatchList });
        }

        [HttpGet]
        public async Task<IActionResult> Seleccionar(int idPelicula)
        {
            var idUsuario = ObtenerIdUsuario();
            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = await _watchListService.ObtenerSeleccionAsync(idUsuario.Value, idPelicula);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Seleccionar(WatchListSeleccionDto model)
        {
            var idUsuario = ObtenerIdUsuario();
            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var resultado = await _watchListService.GuardarSeleccionAsync(idUsuario.Value, model);
            if (!resultado.Exitoso)
            {
                var seleccion = await _watchListService.ObtenerSeleccionAsync(
                    idUsuario.Value,
                    model.IdPelicula);

                if (seleccion == null)
                {
                    return NotFound();
                }

                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                return View(seleccion);
            }

            TempData["Mensaje"] = resultado.Mensaje;
            return RedirectToAction("Detalle", "Peliculas", new { id = model.IdPelicula });
        }

        private int? ObtenerIdUsuario()
        {
            return HttpContext.Session.GetInt32("IdUsuario");
        }
    }
}
