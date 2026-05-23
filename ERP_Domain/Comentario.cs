using Proclin.Models;

namespace ERP_Domain
{
    public class Comentario : BaseModel
    {
        public int IdComentario { get; set; }
        public string Conteudo { get; set; }
        public DateTime DataPublicacao { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public string ExternalNewsId { get; set; }

        public Comentario() { }

        public Comentario(int idUsuario, string conteudo, string externalNewsId)
        {
            this.IdUsuario = idUsuario;
            this.Conteudo = conteudo;
            this.ExternalNewsId = externalNewsId;
            this.DataPublicacao = DateTime.Now;
            SetUsuarioInclusao("Sistema");
        }

        public void Removeer(string usuarioExclusao)
        {
            SetUsuarioExclusao(usuarioExclusao);
        }


    }
}
