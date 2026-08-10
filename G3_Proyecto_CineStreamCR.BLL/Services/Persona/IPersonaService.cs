using G3_Proyecto_CineStreamCR.BLL.Dtos.Personas;

namespace G3_Proyecto_CineStreamCR.BLL.Services.Persona
{
    // Consulta de solo lectura del perfil público de una persona,
    // necesaria para la navegación desde el detalle de película.
    public interface IPersonaService
    {
        Task<PersonaDetalleDto?> ObtenerDetalleAsync(int idPersona);
    }
}
