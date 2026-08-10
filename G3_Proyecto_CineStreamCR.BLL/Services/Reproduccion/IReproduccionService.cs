using System.Threading.Tasks;

namespace G3_Proyecto_CineStreamCR.BLL.Services.Reproduccion
{
    public interface IReproduccionService
    {
        Task<int> ObtenerProgresoAsync(int idUsuario, int idPelicula);
        Task<bool> GuardarProgresoAsync(int idUsuario, int idPelicula, int segundoActual);
    }
}
