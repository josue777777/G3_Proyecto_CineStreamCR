using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G3_Proyecto_CineStreamCR.BLL.Dtos
{
    public class CalificacionDto
    {
        public int IdCalificacion { get; set; }
        public int Puntuacion { get; set; }
        public string? Resena { get; set; }
        public int IdPelicula { get; set; }
        public int IdUsuario { get; set; }
        public string? NombreUsuario { get; set; }
    }
}
