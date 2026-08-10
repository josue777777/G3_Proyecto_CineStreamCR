namespace G3_Proyecto_CineStreamCR.DAL.Repositorios.Persona
{
    // Acceso de solo lectura al perfil público de una persona
    // (director y/o actor), necesario para la navegación desde el
    // detalle de película. La gestión completa de personas pertenece
    // a su propio módulo.
    public interface IPersonaRepositorio
    {
        Task<List<Entidades.Persona>> ObtenerTodasAsync();

        Task<Entidades.Persona?> ObtenerDetalleAsync(int idPersona);
    }
}
