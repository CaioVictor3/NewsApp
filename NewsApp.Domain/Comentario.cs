namespace NewsApp.Domain
{
    public class Comentario
    {
        public int IdComentario { get; set; }
        public string Conteudo { get; set; }
        public DateTime DataComentario { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public int IdNoticia { get; set; }
        public Noticia Noticia { get; set; }
        public DateTime DataInclusao { get; set; }
        public string Situacao { get; set; }

        public Comentario() { }

        public Comentario(int idUsuario, int idNoticia, string conteudo)
        {
            IdUsuario = idUsuario;
            IdNoticia = idNoticia;
            Conteudo = conteudo;
            DataComentario = DateTime.Now;
            DataInclusao = DateTime.Now;
            Situacao = "Ativo";
        }

        public void Atualizar(string conteudo)
        {
            Conteudo = conteudo;
        }

        public void Remover()
        {
            Situacao = "Excluido";
        }
    }
}
