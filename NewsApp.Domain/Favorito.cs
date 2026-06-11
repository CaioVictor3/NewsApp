using Proclin.Models;

namespace NewsApp.Domain
{
    public class Favorito : BaseModel
    {
        public int IdFavorito { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public int IdNoticia { get; set; }
        public Noticia Noticia { get; set; }

        public Favorito() { }

        public Favorito(int idUsuario, int idNoticia)
        {
            IdUsuario = idUsuario;
            IdNoticia = idNoticia;
            SetUsuarioInclusao("Sistema");
        }
    }
}
