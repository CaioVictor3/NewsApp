namespace ERP_Application.Models.Comentario
{
    public class AtualizarComentarioRequestModel
    {
        public int IdComentario { get; set; }
        public string Comentario { get; set; }
        public int IdUsuario { get; set; }
        public int IdNoticia { get; set; }
    }
}
