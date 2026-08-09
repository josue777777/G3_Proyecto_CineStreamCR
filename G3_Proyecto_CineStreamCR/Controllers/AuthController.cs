using G3_Proyecto_CineStreamCR.BLL.Dtos.Usuarios;
using G3_Proyecto_CineStreamCR.BLL.Services.Usuario;
using G3_Proyecto_CineStreamCR.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace G3_Proyecto_CineStreamCR.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUsuarioServicio _usuarioServicio;

        public AuthController(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        // Muestra la pantalla de inicio de sesión.
        [HttpGet]
        public IActionResult Login()
        {
            var idUsuario =
                HttpContext.Session.GetInt32("IdUsuario");

            // Si ya existe una sesión activa,
            // no vuelve a mostrar el Login.
            if (idUsuario != null)
            {
                return RedirectToAction(
                    "Index",
                    "Home");
            }

            return View();
        }

        // Procesa el inicio de sesión.
        [HttpPost]
        public async Task<IActionResult> Login(
            LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var loginDto = new LoginDto
            {
                Identificador = model.Identificador,
                Contrasenna = model.Contrasenna
            };

            var resultado =
                await _usuarioServicio.LoginAsync(loginDto);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError(
                    string.Empty,
                    resultado.Mensaje);

                return View(model);
            }

            // Guarda únicamente la información básica
            // necesaria para identificar al usuario.
            HttpContext.Session.SetInt32(
                "IdUsuario",
                resultado.Usuario!.IdUsuario);

            HttpContext.Session.SetString(
                "NombreUsuario",
                resultado.Usuario.NombreUsuario);

            return RedirectToAction(
                "Index",
                "Home");
        }

        // Cierra la sesión del usuario actual.
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                "Login",
                "Auth");
        }
    }
}