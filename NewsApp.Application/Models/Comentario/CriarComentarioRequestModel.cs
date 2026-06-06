namespace NewsApp.Application.Models.Comentario
{
    public class CriarComentarioRequestModel
    {
        public string Comentario { get; set; }
        public int IdNoticia { get; set; }
        public int IdUsuario { get; set; }
    }
}
