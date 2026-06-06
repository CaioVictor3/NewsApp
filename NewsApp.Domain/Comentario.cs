using Proclin.Models;

namespace NewsApp.Domain
{
    public class Comentario : BaseModel
    {
        public int IdComentario { get; set; }
        public string Conteudo { get; set; }
        public DateTime DataComentario { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public int IdNoticia { get; set; }
        public Noticia Noticia { get; set; }

        public Comentario() { }

        public Comentario(int idUsuario, int idNoticia, string conteudo)
        {
            IdUsuario = idUsuario;
            IdNoticia = idNoticia;
            Conteudo = conteudo;
            DataComentario = DateTime.Now;
            SetUsuarioInclusao("Sistema");
        }

        public void Atualizar(string conteudo, string usuarioAlteracao)
        {
            Conteudo = conteudo;
            SetUsuarioAlteracao(usuarioAlteracao);
        }

        public void Remover(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }
    }
}
