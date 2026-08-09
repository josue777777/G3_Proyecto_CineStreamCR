using G3_Proyecto_CineStreamCR.BLL.Dtos.Usuarios;
using EntidadUsuario = G3_Proyecto_CineStreamCR.DAL.Entidades.Usuario;

namespace G3_Proyecto_CineStreamCR.BLL.Services.Usuario
{
    public interface IUsuarioServicio
    {
        Task<(bool Exitoso, string Mensaje, EntidadUsuario? Usuario)>
            LoginAsync(LoginDto loginDto);
    }
}