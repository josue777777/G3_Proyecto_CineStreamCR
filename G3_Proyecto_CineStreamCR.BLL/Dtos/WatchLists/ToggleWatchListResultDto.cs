namespace G3_Proyecto_CineStreamCR.BLL.Dtos.WatchLists
{
    // Resultado de agregar/quitar una película de la WatchList
    // rápida del usuario, usado desde el catálogo y el detalle.
    public class ToggleWatchListResultDto
    {
        public bool Exitoso { get; set; }

        public bool EnWatchList { get; set; }

        public string Mensaje { get; set; } = string.Empty;
    }
}
