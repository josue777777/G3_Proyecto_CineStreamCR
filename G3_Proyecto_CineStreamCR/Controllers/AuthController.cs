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

        // Muestra la pantalla de autenticación.
        [HttpGet]
        public IActionResult Login()
        {
            var idUsuario =
                HttpContext.Session.GetInt32("IdUsuario");

            // Si ya existe una sesión activa,
            // redirige al usuario al Home.
            if (idUsuario != null)
            {
                return RedirectToAction(
                    "Index",
                    "Home");
            }

            return View(new AuthViewModel());
        }

        // Procesa únicamente el formulario de inicio de sesión.
        [HttpPost]
        public async Task<IActionResult> Login(
            [Bind(Prefix = "Login")] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var authViewModel = new AuthViewModel
                {
                    Login = model,
                    MostrarRegistro = false
                };

                return View(authViewModel);
            }

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

                var authViewModel = new AuthViewModel
                {
                    Login = model,
                    MostrarRegistro = false
                };

                return View(authViewModel);
            }

            // Guarda únicamente la información necesaria
            // para identificar al usuario autenticado.
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

        // Procesa únicamente el formulario de registro.
        [HttpPost]
        public async Task<IActionResult> Register(
            [Bind(Prefix = "Registro")] RegistroViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var authViewModel = new AuthViewModel
                {
                    Registro = model,
                    MostrarRegistro = true
                };

                return View(
                    "Login",
                    authViewModel);
            }

            var registroDto = new RegistroDto
            {
                NombreUsuario = model.NombreUsuario,
                Correo = model.Correo,
                Contrasenna = model.Contrasenna
            };

            var resultado =
                await _usuarioServicio.RegistrarAsync(
                    registroDto);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError(
                    string.Empty,
                    resultado.Mensaje);

                var authViewModel = new AuthViewModel
                {
                    Registro = model,
                    MostrarRegistro = true
                };

                return View(
                    "Login",
                    authViewModel);
            }

            TempData["RegistroExitoso"] =
                "Cuenta creada correctamente. Ya puede iniciar sesión.";

            return RedirectToAction(nameof(Login));
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