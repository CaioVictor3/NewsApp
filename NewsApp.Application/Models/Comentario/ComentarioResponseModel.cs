namespace NewsApp.Application.Models.Comentario
{
    public class ComentarioResponseModel
    {
        public int IdComentario { get; set; }
        public string Comentario { get; set; }
        public int IdUsuario { get; set; }
        public int IdNoticia { get; set; }
        public DateTime DataComentario { get; set; }
    }
}
