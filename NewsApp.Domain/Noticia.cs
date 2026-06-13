namespace NewsApp.Domain
{
    public class Noticia
    {
        public int IdNoticia { get; set; }
        public string FonteId { get; set; } = string.Empty;
        public string FonteNome { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string UrlImagem { get; set; } = string.Empty;
        public DateTime DataPublicacao { get; set; }
        public string Conteudo { get; set; } = string.Empty;
        public DateTime DataInclusao { get; set; }

        public Noticia() { }

        public Noticia(
            string fonteId,
            string fonteNome,
            string autor,
            string titulo,
            string descricao,
            string url,
            string urlImagem,
            DateTime dataPublicacao,
            string conteudo)
        {
            FonteId = fonteId;
            FonteNome = fonteNome;
            Autor = autor;
            Titulo = titulo;
            Descricao = descricao;
            Url = url;
            UrlImagem = urlImagem;
            DataPublicacao = dataPublicacao;
            Conteudo = conteudo;
            DataInclusao = DateTime.Now;
        }
    }
}
