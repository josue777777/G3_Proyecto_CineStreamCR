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
            return View();
        }

        // Procesa el inicio de sesión.
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
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

            // La sesión se configurará en el siguiente paso.
            // Por ahora solamente confirmamos que la autenticación funciona.

            return RedirectToAction(
                "Index",
                "Home");
        }
    }
}