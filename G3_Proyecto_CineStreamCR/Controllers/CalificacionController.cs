using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using G3_Proyecto_CineStreamCR.BLL.Dtos;
// using G3_Proyecto_CineStreamCR.BLL.Services; // Descomenta cuando tengas el servicio

namespace G3_Proyecto_CineStreamCR.Controllers
{
    public class CalificacionController : Controller
    {
        // Aquí inyectarías tu servicio: private readonly ICalificacionService _calificacionService;

        [HttpPost]
        public IActionResult GuardarResena([FromBody] CalificacionDto calificacionDto)
        {
            // 1. Validar que el usuario tenga sesión (Como lo hizo Josué)
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuario == null)
            {
                return Unauthorized(new { mensaje = "Debes iniciar sesión para calificar." });
            }

            // 2. Asignar el usuario logueado al DTO
            calificacionDto.IdUsuario = idUsuario.Value;

            // 3. Validar regla de negocio (KISS / Cláusula de guarda que pide el profe)
            if (calificacionDto.Puntuacion < 1 || calificacionDto.Puntuacion > 10)
            {
                return BadRequest(new { mensaje = "La puntuación debe estar entre 1 y 10." });
            }

            // 4. Aquí llamas a tu capa BLL para guardar en base de datos.
            // Ejemplo: _calificacionService.AgregarResena(calificacionDto);

            return Ok(new { mensaje = "¡Reseña guardada exitosamente!" });
        }
    }
}